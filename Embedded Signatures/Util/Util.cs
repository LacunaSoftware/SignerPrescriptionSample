using Lacuna.Signer.Api;
using Lacuna.Signer.Api.Documents;
using Lacuna.Signer.Api.FlowActions;
using Lacuna.Signer.Api.Users;
using Lacuna.Signer.Client;

namespace Embedded_Signatures
{
    public class Util
    {
        public static async Task<string> UpdateAndEmbedSignatureRequest(Boolean allowElectronicSignature = false)
        {
            var signerClient = new SignerClient("https://signer-lac.azurewebsites.net", "API Sample App|43fc0da834e48b4b840fd6e8c37196cf29f919e5daedba0f1a5ec17406c13a99");
            var filePath = "sample.pdf";
            var fileName = Path.GetFileName(filePath);
            var file = System.IO.File.ReadAllBytes(filePath);
            var uploadModel = await signerClient.UploadFileAsync(fileName, file, "application/pdf");
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
                AllowElectronicSignature = allowElectronicSignature
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
 
    }
}
