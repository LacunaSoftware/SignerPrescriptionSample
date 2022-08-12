using Microsoft.AspNetCore.Mvc.Rendering;
namespace Embedded_Signatures.Models
{
    public class CreatePrescriptionModel
    {
        public string Identifier { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string PatientName { get; set; }
        public string MedicationName { get; set; }
        public string MedicationDosage { get; set; }
        public string MedicationQuantity { get; set; }
        public bool AllowElectronicSignature { get; set; }
        public string UF { get; set; }
        public string CRM { get; set; }
    }
}
