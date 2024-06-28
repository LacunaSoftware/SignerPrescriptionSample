using iTextSharp.text.pdf;
using Lacuna.RestPki.Api.PadesSignature;
using Lacuna.RestPki.Api;
using Lacuna.RestPki.Client;
using Lacuna.Signer.Api;
using Lacuna.Signer.Client;
using PkiSuiteAspNetMvcSample.Classes;

namespace Embedded_Signatures.Services {
    public class SignerService
    {
        private readonly IWebHostEnvironment env;
        private readonly SignerClient client;
        private readonly RestPkiClient restPkiClient;
        private string url;
		public static string AppDataPath = "~/App_Data";

		public SignerService(
            IWebHostEnvironment env
        )
        {
            this.env = env;
            url = "https://signer-lac.azurewebsites.net";
            client = new SignerClient(url, "API Sample App|43fc0da834e48b4b840fd6e8c37196cf29f919e5daedba0f1a5ec17406c13a99");
            restPkiClient = new RestPkiClient("https://pki.rest/", "o-efxzcAO1hiK2KwTg9PcW0833MIAmcdvaHpsvYbSPGd245gkfS2IZRbdGV4pocDop9NyRIKpC5F1YUTjmhJWKgJVGb0u_flHbbvIWpObDQM44WfHpq2lP4TQsEZM4qqXEGniAsdlwjMm6xGFh_OUj-tSPSbyVxK1SO9cnCxhiQpqPTSHx1BnswWia0jFKtHSSIxUei8jyUq8AyRgnR6KMcbkqVJlOlP0TQ7l0IB1mea6_TGOghnxNPBjikVJTtkQ2ayRe9VZihNw2g03nH_qMGrQB1ZwBnphK9sRXXiHO4wn21ip0giNs3YkCIhndIi0pC7uxTtnIi0UaKhlYaik6rwFl5hoxC-170NfB59bPyq6dOgxcJVjhIXTc2p3-qwCXfRvlpVDh3JJ0MqiUGiDmIeus7aj5i5bXN--vG8Hi-Fj5_Io3SerFuZrpqfWLgS6foAVU2PuU2xuWUtKRsv9GUMSx5WbclnlhTqKnwyCtEcUnD1FMDUB6J3CewnocBEJhZ1ng");
        }

        public async Task<MemoryStream> CreateDocument(string name, string medicine)
        {
            return CreatePrescriptionPdf(name, medicine);
        }

        public async Task<ClientSideSignatureInstructions> StartSignature(MemoryStream fileStream, byte[] certificate) {
            var signatureStarter = new PadesSignatureStarter(restPkiClient) {

                // Set the unit of measurement used to edit the pdf marks and visual representations.
                MeasurementUnits = PadesMeasurementUnits.Centimeters,

                // Set the signature policy.
                SignaturePolicyId = StandardPadesSignaturePolicies.Basic,
                // Set the security context to be used to determine trust in the certificate chain. We have

                // encapsulated the security context choice on Util.cs.
                SecurityContextId = StandardSecurityContexts.PkiBrazil,
                SignerCertificate = certificate,
                VisualRepresentation = PadesVisualElements.GetVisualRepresentationForRestPki(env, restPkiClient),
            };

            // Set the file to be signed.
            signatureStarter.SetPdfToSign(fileStream);

            var res = await signatureStarter.StartAsync();
            return res;
        }

        public SignatureResult FinishSignature(string token, byte[] signedHash) {

            // Get an instance of the PadesSignatureFinisher2 class, responsible for completing the
            // signature process.
            var signatureFinisher = new PadesSignatureFinisher2(restPkiClient) {

                // Set the token for this signature. (rendered in a hidden input field, see the view)
                Token = token,
                Signature = signedHash
            };

            // Call the Finish() method, which finalizes the signature process and returns a
            // SignatureResult object.
            var result = signatureFinisher.Finish();

            return result;
        }

        public async Task<string> GetDownloadUrl(Guid documentId)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt");
            var response = await client.GetDocumentDownloadTicketAsync(documentId, DocumentTicketType.PrinterFriendlyVersion);

            return new Uri(new Uri(url), response.Location).AbsoluteUri;
        }

        private MemoryStream CreatePrescriptionPdf(string name, string medicine)
        {
            var pdfFile = System.IO.File.ReadAllBytes(Path.Combine(env.ContentRootPath, "Template-Prescricao.pdf"));
            var reader = new PdfReader(pdfFile);
            var stream = new MemoryStream();
            var stamper = new PdfStamper(reader, stream);
            stamper.AcroFields.SetField("Nome", name);
            stamper.AcroFields.SetField("Medicamentos", medicine);
            stamper.FormFlattening = true;
            stamper.Close();
            stream.Position = 0;
            return stream;
        }


        public string Store(Stream stream, string extension = "", string filename = null) {

            
            // Guarantees that the "App_Data" folder exists.
            if (!Directory.Exists(AppDataPath)) {
				Directory.CreateDirectory(AppDataPath);
			}

			if (string.IsNullOrEmpty(filename)) {
				filename = Guid.NewGuid() + extension;
			}

			var path = Path.Combine(AppDataPath, filename.Replace("_", "."));
			using (var fileStream = System.IO.File.Create(path)) {
				stream.CopyTo(fileStream);
			}

			return filename.Replace(".", "_");
			// Note: we're passing the filename argument with "." as "_" because of limitations of ASP.NET MVC.
		}

        public static byte[] Read(string fileId, out string filename) {

			if (string.IsNullOrEmpty(fileId)) {
				throw new ArgumentNullException("fileId");
			}
			filename = fileId.Replace("_", ".");
			// Note: we're receiving the fileId argument with "_" as "." because of limitations of ASP.NET MVC.

			using (var inputStream = OpenRead(filename)) {
				using (var buffer = new MemoryStream()) {
					inputStream.CopyTo(buffer);
					return buffer.ToArray();
				}
			}
		}

		public static Stream OpenRead(string filename) {

			if (string.IsNullOrEmpty(filename)) {
				throw new ArgumentNullException("fileId");
			}

			var path = Path.Combine(AppDataPath, filename);
			var fileInfo = new FileInfo(path);
			if (!fileInfo.Exists) {
				throw new FileNotFoundException("File not found: " + filename);
			}
			return fileInfo.OpenRead();
		}

	}
}
