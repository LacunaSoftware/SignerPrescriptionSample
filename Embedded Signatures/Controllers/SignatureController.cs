using Lacuna.Signer.Api;
using Lacuna.Signer.Api.Documents;
using Lacuna.Signer.Api.FlowActions;
using Lacuna.Signer.Api.Users;
using Lacuna.Signer.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Embedded_Signatures.Controllers
{
    public class SignatureController : Controller
    {
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["endpoint"] = "https://signer-lac.azurewebsites.net";
            ViewData["token"] = "API Sample App|43fc0da834e48b4b840fd6e8c37196cf29f919e5daedba0f1a5ec17406c13a99";

            var participantUser = new ParticipantUserModel()
            {
                Name = "Jack Bauer",
                Email = "jack.bauer@mailinator.com",
                Identifier = "75502846369"
            };
            var flowActionCreateModel = new FlowActionCreateModel()
            {
                Type = FlowActionType.Signer,
                User = participantUser
            };

            
            //var documentRequest = new CreateDocumentRequest()
            //{
            //    Files = new List<FileUploadModel>() { fileUploadModel },
            //    FlowActions = new List<FlowActionCreateModel>() { flowActionCreateModel }
            //};
            //var result = (await SignerClient.CreateDocumentAsync(documentRequest)).First();
            //var actionUrlRequest = new ActionUrlRequest()
            //{
            //    Identifier = participantUser.Identifier
            //};
            //var actionUrlResponse = await SignerClient.GetActionUrlAsync(result.DocumentId, actionUrlRequest);
            //ViewData["response"] = actionUrlResponse.EmbedUrl; 

            //Console.WriteLine(actionUrlResponse.EmbedUrl);

            return View();
        }
      
        private async Task<FileUploadModel> uploadSampleFile(string endpoint, string token)
        {
           
            var SignerClient = new SignerClient(endpoint, token);
            var filePath = "sample.pdf";
            var fileName = Path.GetFileName(filePath);
            var file = System.IO.File.ReadAllBytes(filePath);
            var uploadModel = await SignerClient.UploadFileAsync(fileName, file, "application/pdf");
            var fileUploadModel = new FileUploadModel(uploadModel) { DisplayName = "Embedded Signature Sample" };
            return fileUploadModel;
        }
    }
}
