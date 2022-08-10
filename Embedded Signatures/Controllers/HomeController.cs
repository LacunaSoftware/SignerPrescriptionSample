using Embedded_Signatures.Models;
using Embedded_Signatures.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Embedded_Signatures.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignerService signerService;

        

        public HomeController(ILogger<HomeController> logger, SignerService signerService)
        {
            _logger = logger;
            this.signerService = signerService;
        }

        public IActionResult Index()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Prescription([FromBody] CreatePrescriptionModel prescription)
        {
            var embed = await signerService.CreateDocument(prescription.PatientName, prescription.Name, prescription.Email, prescription.MedicationName, prescription.Identifier, prescription.AllowElectronicSignature);

            return Json(new { embedUrl = embed });
        }

        public async Task<IActionResult> Prescription(Guid id)
        {
            var url = await signerService.GetDownloadUrl(id);

            return Json(new { url });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
