using iTextSharp.text.pdf;
using Lacuna.Signer.Api;
using Lacuna.Signer.Api.DocumentMark;
using Lacuna.Signer.Api.Documents;
using Lacuna.Signer.Api.FlowActions;
using Lacuna.Signer.Api.Users;
using Lacuna.Signer.Client;

namespace Embedded_Signatures
{
    public class Util
    {
        public static async Task<string> UploadDocument(string name, string medicine)
        {
            var signerClient = new SignerClient("https://signer-lac.azurewebsites.net", "API Sample App|43fc0da834e48b4b840fd6e8c37196cf29f919e5daedba0f1a5ec17406c13a99");
            var fileStream = CreatePrescriptionPdf(name, medicine);
            var filePath = "Template-Prescricao.pdf";
            var fileName = Path.GetFileName(filePath);
            var uploadModel = await signerClient.UploadFileAsync(fileName, fileStream, "application/pdf");
            var fileUploadModel = new FileUploadModel(uploadModel) { DisplayName = "Embedded Signature Sample" };

            var participantUser = new ParticipantUserModel()
            {
                Name = "Alan Mathison Turing",
                Email = "lcnturing@mailinator.com",
                Identifier = "56072386105"
            };
            var flowActionCreateModel = new FlowActionCreateModel()
            {
                Type = FlowActionType.Signer,
                User = participantUser,
                PrePositionedMarks = new List<PrePositionedDocumentMarkModel>
                {
                    new PrePositionedDocumentMarkModel()
                    {
                        Type = DocumentMarkType.SignatureVisualRepresentation,
                        UploadId = fileUploadModel.Id,
                        TopLeftX = 228,
                        TopLeftY = 656,
                        Width = 170.0,
                        Height = 94.0,
                    }
            }
            };

            var documentRequest = new CreateDocumentRequest()
            {
                Files = new List<FileUploadModel>() { fileUploadModel },
                FlowActions = new List<FlowActionCreateModel>() { flowActionCreateModel }
            };
            var result = (await signerClient.CreateDocumentAsync(documentRequest)).First();
            var actionUrlRequest = new ActionUrlRequest()
            {
                Identifier = participantUser.Identifier
            };
            var actionUrlResponse = await signerClient.GetActionUrlAsync(result.DocumentId, actionUrlRequest);

            return actionUrlResponse.EmbedUrl;
        }
        static MemoryStream CreatePrescriptionPdf(string name, string medicine)
        {
            var pdfFile = File.ReadAllBytes("Template-Prescricao.pdf");
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
