using iTextSharp.text.pdf;
using Lacuna.Signer.Api;
using Lacuna.Signer.Api.DocumentMark;
using Lacuna.Signer.Api.Documents;
using Lacuna.Signer.Api.FlowActions;
using Lacuna.Signer.Api.Users;
using Lacuna.Signer.Api.HealthDocuments;
using Lacuna.Signer.Client;

namespace Embedded_Signatures.Services
{
    public class SignerService
    {
        private readonly IWebHostEnvironment env;
        private readonly SignerClient client;
        private string url;

        public SignerService(
            IWebHostEnvironment env
        )
        {
            this.env = env;
            url = "https://signer-lac.azurewebsites.net";
            client = new SignerClient(url, "API Sample App|43fc0da834e48b4b840fd6e8c37196cf29f919e5daedba0f1a5ec17406c13a99");
        }

        public async Task<string> CreateDocument(
            string patientName,
            string crm, 
            string uf, 
            string medicationDosage,
            string medicationQuantity,
            string name,
            string email,
            string medicine,
            string identifier,
            bool allowElectronicSignature = false
            )
        {
            var fileStream = CreatePrescriptionPdf(patientName, medicine);
            var filePath = "Template-Prescricao.pdf";
            var fileName = Path.GetFileName(filePath);
            var uploadModel = await client.UploadFileAsync(fileName, fileStream, "application/pdf");
            var fileUploadModel = new FileUploadModel(uploadModel) { DisplayName = "Embedded Signature Sample" };

            var participantUser = new ParticipantUserModel();

            participantUser.Email = email;
            participantUser.Name = name;
            participantUser.Identifier = identifier;

            var flowActionCreateModel = new FlowActionCreateModel()
            {
                Type = FlowActionType.Signer,
                User = participantUser,
                AllowElectronicSignature = allowElectronicSignature,
                PrePositionedMarks = new List<PrePositionedDocumentMarkModel>
                {
                    new PrePositionedDocumentMarkModel()
                    {
                        Type = DocumentMarkType.SignatureVisualRepresentation,
                        UploadId = fileUploadModel.Id,
                        TopLeftX = 228,
                        TopLeftY = 652,
                        Width = 170.0,
                        Height = 40.0,
                    }
                }
            };

            // Item info for health document (patient name, medication and dosage)
            var itemModel = new HealthItemModel()
            {
                Name = medicine,
                Description = medicationDosage,
                Description2 = medicationQuantity
            };

            var documentRequest = new CreateDocumentRequest()
            {
                Files = new List<FileUploadModel>() { fileUploadModel },
                FlowActions = new List<FlowActionCreateModel>() { flowActionCreateModel },
                Type = DocumentTypes.Prescription,
                AdditionalInfo = new DocumentAdditionalInfoData()
                {
                    HealthData = new HealthDocumentData()
                    {
                        Professional = new HealthProfessionalModel()
                        {
                            Name = name,
                            Id = crm,
                            Region = uf
                        },
                        Items = new List<HealthItemModel>() { itemModel }
                    },
                    Fields = new Dictionary<string, string>()
                    {
                        { "Nome", patientName }, 
                        { "Medicamentos", $"{medicine}\r\n{medicationDosage}\r\n{medicationQuantity}" }
                    }
                }
            };
            var result = (await client.CreateDocumentAsync(documentRequest)).First();
            var actionUrlRequest = new ActionUrlRequest()
            {
                Identifier = participantUser.Identifier
            };
            var actionUrlResponse = await client.GetActionUrlAsync(result.DocumentId, actionUrlRequest);

            return actionUrlResponse.EmbedUrl;
        }

        public async Task<string> GetDownloadUrl(Guid documentId)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt");
            var response = await client.GetDocumentDownloadTicketAsync(documentId, DocumentTicketType.Original);

            return new Uri(new Uri(url), response.Location).AbsoluteUri;
        }

        private MemoryStream CreatePrescriptionPdf(string name, string medicine)
        {
            var pdfFile = File.ReadAllBytes(Path.Combine(env.ContentRootPath, "Template-Prescricao.pdf"));
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



    }
}
