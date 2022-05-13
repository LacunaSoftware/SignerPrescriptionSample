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
                Console.WriteLine("Option: " + model.MedicationOptions);
                Console.WriteLine("Name: " + model.MedicationName);
                return View(model);
            }

            return View();
        }
    }
}