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

        public async Task<IActionResult> IndexAsync()
        {
            var embed = new SignatureViewModel();
            //embed.embedUrlDigital = await Util.UpdateAndEmbedSignatureRequest(false);
            //embed.embedUrlEletronic = await Util.UpdateAndEmbedSignatureRequest(true);
            return View(embed);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Prescription(PrescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("kk deu bom man");
                Console.WriteLine("Name: " + model.PatientName);
                Console.WriteLine("Option: " + model.MedicationName);
                
                return View();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PrescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Console.WriteLine("kk deu bom man");
            Console.WriteLine("Name: " + model.PatientName);
            Console.WriteLine("Option: " + model.MedicationName);

            return View();
        }
    }
}