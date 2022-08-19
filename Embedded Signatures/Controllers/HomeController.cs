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
            var embed = await signerService.CreateDocument(
                patientName:prescription.PatientName,
                patientIdentifier:prescription.PatientIdentifier,
                crm:prescription.CRM,
                uf: prescription.UF,
                medicationDosage:prescription.MedicationDosage,
                medicationQuantity:prescription.MedicationQuantity,
                name:prescription.Name,
                email:prescription.Email,
                medicine:prescription.MedicationName,
                identifier:prescription.Identifier
                // This has been removed since prescription no longer uses electronic signature for Prescription documents, 
                // feel free to uncomment if necessary
                //allowElectronicSignature:prescription.AllowElectronicSignature 
                );

            return Json(new { embedUrl = embed });
        }

        public async Task<IActionResult> Prescription(Guid id)
        {
            var url = await signerService.GetDownloadUrl(id);

            return Json(new { url });
        }

        public async Task<IActionResult> OpenPrescription(string key)
        {
            var url = await signerService.GetPrescríptionViewUrl(key);

            return Json(new { url });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
