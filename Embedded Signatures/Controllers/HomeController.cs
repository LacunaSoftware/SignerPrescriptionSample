using Embedded_Signatures.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Embedded_Signatures.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(CreatePrescriptionModel prescription)
        {
            var embed = await Util.UploadDocument(prescription.PatientName, prescription.MedicationName);
            ViewBag.embed = embed;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}