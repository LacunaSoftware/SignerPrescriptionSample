using Embedded_Signatures.Models;
using Embedded_Signatures.Services;
using Lacuna.Cloudhub.Api;
using Lacuna.Cloudhub.Client;
using Lacuna.RestPki.Api.PadesSignature;
using Lacuna.RestPki.Api;
using Lacuna.RestPki.Client;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.util;

namespace Embedded_Signatures.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly SignerService signerService;
        private CloudhubClient cloudhubClient = new CloudhubClient("https://cloudhub.lacunasoftware.com/", "ftulJuCci2cUjkdecTu/fAsUePv8ahga+CRvgKPFmCc=");

        public HomeController(ILogger<HomeController> logger, SignerService signerService) {
            _logger = logger;
            this.signerService = signerService;
        }

        public IActionResult Index() {
            var model = new CreateCloudhubSessionModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CloudLogin(string cpf) {
            var plainCpf = Regex.Replace(cpf, @"[.-]+", "");

            /*Create a SessionRequest for Cloudhub to identify the certificates available 
            *This action will be called after the user press the button "Search" on index page.It will
            *search for all PSCs that have a certificate with the provided identifier.Thus, it will start the
            *authentication process and return a session.
            */
            var res = await cloudhubClient.CreateSessionAsync(new Lacuna.Cloudhub.Api.SessionCreateRequest {
                Identifier = cpf,
                RedirectUri = "http://localhost:54123/Home/Prescription",
                Type = Lacuna.Cloudhub.Api.TrustServiceSessionTypes.SignatureSession
            });

            return View(new CreateCloudhubSessionModel() {
                Services = res.Services,
                CPF = plainCpf,
            });
        }

        // GET: /Home/Prescription
        public ActionResult Prescription(string session, CreatePrescriptionModel model) {
            // Use the session parameter as needed
            model.Session = session;
            // Return the view
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Result(CreatePrescriptionModel prescription) {
            var filestream = await signerService.CreateDocument(prescription.PatientName, prescription.MedicationName);

            var cert = await cloudhubClient.GetCertificateAsync(prescription.Session);
            var startRes = await signerService.StartSignature(filestream, cert);
            var signHashRes = await cloudhubClient.SignHashAsync(new SignHashRequest() {
                Hash = startRes.ToSignHash,
                DigestAlgorithmOid = startRes.DigestAlgorithmOid,
                Session = prescription.Session,
            });

            var finishRes = signerService.FinishSignature(startRes.Token, signHashRes);
            string fileId;
            using (var resultStream = finishRes.OpenRead()) {
                fileId = signerService.Store(resultStream, ".pdf");
            }

            var model = new SignatureInfoModel {
                File = fileId,
                SignerCertificate = finishRes.Certificate
            };

            // Return JSON with the URL to redirect to
            return View("Result", model);
        }

		// GET Download/File/{id}
		[HttpGet]
		public ActionResult File(string id) {
			byte[] content;


			string filename = "";
			try {
				content = SignerService.Read(id, out filename);
			} catch (FileNotFoundException) {
				throw new FileNotFoundException("File not found: " + filename);
			}

			return File(content, "application/pdf");
		}

		public static Stream OpenRead(string filename) {

			if (string.IsNullOrEmpty(filename)) {
				throw new ArgumentNullException("fileId");
			}

			var path = Path.Combine(SignerService.AppDataPath, filename);
			var fileInfo = new FileInfo(path);
			if (!fileInfo.Exists) {
				throw new FileNotFoundException("File not found: " + filename);
			}
			return fileInfo.OpenRead();
		}

	}
}
