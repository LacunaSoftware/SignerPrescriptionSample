using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SignerPrescriptionSample.Models
{
    public class CreatePrescriptionModel
    {
        [Required (ErrorMessage="É necessário informar um CPF")]
        public string Identifier { get; set; }
        [Required(ErrorMessage = "É necessário informar um nome")]
        public string Name { get; set; }
        [Required(ErrorMessage = "É necessário informar um e-mail válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "É necessário informar o nome do paciente")]
        public string PatientName { get; set; }
        [Required(ErrorMessage = "É necessário informar o CPF do paciente")]
        public string PatientIdentifier { get; set; }
        [Required(ErrorMessage = "É necessário informar o nome do medicamento")]
        public string MedicationName { get; set; }
        [Required(ErrorMessage = "É necessário informar a posologia do medicamento")]
        public string MedicationDosage { get; set; }
        [Required(ErrorMessage = "É necessário informar a quantidade do medicamento")]
        public string MedicationQuantity { get; set; }
        public bool AllowElectronicSignature { get; set; }
        [Required(ErrorMessage = "É necessário informar a unidade federativa")]
        public string UF { get; set; }
        [Required(ErrorMessage = "É necessário informar um CRM válido")]
        public string CRM { get; set; }
    }
}
