using Embedded_Signatures.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Embedded_Signatures.Controllers {
	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger) {
			_logger = logger;
		}

		public async Task<IActionResult> IndexAsync() {
			var embed = new SignatureViewModel();
			embed.embedUrlDigital = await Util.UpdateAndEmbedSignatureRequest(false);
			embed.embedUrlEletronic = await Util.UpdateAndEmbedSignatureRequest(true);
			return View(embed);
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}