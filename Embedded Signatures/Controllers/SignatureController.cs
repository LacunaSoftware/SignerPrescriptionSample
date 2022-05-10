using Microsoft.AspNetCore.Mvc;
using Embedded_Signatures.Controllers;
using Embedded_Signatures.Models;

namespace Embedded_Signatures.Controllers
{
    public class SignatureController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Sign()
        {
            var embed = new SignatureViewModel();
            embed.embedUrlEletronic = await Util.UpdateAndEmbedSignatureRequest(false);
            return View(embed);
        }

        [HttpGet]
        public async Task<IActionResult> SignWithoutPreview()
        {
            var embed = new SignatureViewModel();
            embed.embedUrlEletronic = await Util.UpdateAndEmbedSignatureRequest(false);
            return View(embed);
        }
        [HttpGet]
        public async Task<IActionResult> SignElectronically()
        {
            var embed = new SignatureViewModel();
            embed.embedUrlEletronic = await Util.UpdateAndEmbedSignatureRequest(true);
            return View(embed);
        }
        [HttpGet]
        public async Task<IActionResult> SignElectronicallyWithoutPreview()
        {
            var embed = new SignatureViewModel();
            embed.embedUrlEletronic = await Util.UpdateAndEmbedSignatureRequest(true);
            return View(embed);
        }



    }
}
