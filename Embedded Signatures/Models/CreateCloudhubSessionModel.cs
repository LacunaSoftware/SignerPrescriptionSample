using Lacuna.Cloudhub.Api;

namespace Embedded_Signatures.Models {
    public class CreateCloudhubSessionModel {
        public string CPF { get; set; }
        public List<TrustServiceAuthParametersModel> Services { get; set; }
    }
}
