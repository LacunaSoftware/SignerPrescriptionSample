﻿using SignerPrescriptionSample.Models;
using SignerPrescriptionSample.Services;
using Lacuna.Pki;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace SignerPrescriptionSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly SignerService signerService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, SignerService signerService, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.logger = logger;
            this.signerService = signerService;
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }

        public IActionResult HomePage() {
            return View();
        }



        public IActionResult Index()
        {
            
            var sdkLicense = configuration.GetValue<string>("PkiSDKLicense");

            PkiConfig.LoadLicense(Convert.FromBase64String(sdkLicense));

            var nonceStore = Utils.GetNonceStore(webHostEnvironment);

            var certAuth = new PKCertificateAuthentication(nonceStore);

            var nonce = certAuth.Start();

            var model = new AuthenticationModel()
            {
                Nonce = nonce,
                DigestAlgorithm = PKCertificateAuthentication.DigestAlgorithm.Oid
            };
            var vr = TempData["ValidationResults"] as ValidationResults;
            if (vr != null && !vr.IsValid)
            {
                ModelState.AddModelError("", vr.ToString());
            }

            return View(model);
        }
    

		[HttpPost]
		public IActionResult Index(AuthenticationModel model)
        {

			var nonceStore = Utils.GetNonceStore(webHostEnvironment);

			var certAuth = new PKCertificateAuthentication(nonceStore);

			PKCertificate certificate; 

            var vr = certAuth.Complete(model.Nonce, model.Certificate, model.Signature, Utils.GetTrustArbitrator(), out certificate);

            var validation = new ValidationErrorModel();

            validation.Results = vr;

            if (!vr.IsValid)
            {
                return View("ValidationErrorView", validation);
            }

			return View("Form", new CreatePrescriptionModel()
			{
				UserCert = PKCertificate.Decode(model.Certificate)
			});
		}

		[HttpPost]
        public async Task<IActionResult> Prescription([FromBody]CreatePrescriptionModel prescription)
        {
            var embed = await signerService.CreateDocument(
                patientName: prescription.PatientName,
                patientIdentifier: prescription.PatientIdentifier,
                crm: prescription.CRM,
                uf: prescription.UF,
                medicationDosage: prescription.MedicationDosage,
                medicationQuantity: prescription.MedicationQuantity,
                name: prescription.Name,
                email: prescription.Email,
                medicine: prescription.MedicationName,
                identifier: prescription.Identifier
                );
            return Json(new {embedUrl = embed});
        }

        public async Task<IActionResult> Prescription(Guid id)
        {
            var url = await signerService.GetDownloadUrl(id);

            return Json(new { url });
        }

        public IActionResult GetPrescriptionViewFromDocumentKey(string key)
        {
            var url = signerService.GetPrescríptionViewUrl(key);

            return Json(new { url });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}