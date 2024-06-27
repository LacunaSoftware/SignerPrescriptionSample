using Lacuna.RestPki.Client;
namespace Embedded_Signatures.Models
{
	public class SignatureInfoModel {
		public string File { get; set; }
		public PKCertificate SignerCertificate { get; set; }
	}
}