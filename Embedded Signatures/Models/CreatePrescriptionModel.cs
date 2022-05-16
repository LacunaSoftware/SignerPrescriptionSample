using Microsoft.AspNetCore.Mvc.Rendering;
namespace Embedded_Signatures.Models
{
    public class CreatePrescriptionModel
    {
        public string PatientName { get; set; }
        public string MedicationName { get; set; }
        public List<SelectListItem> MedicationOptions = new()
        {
             new SelectListItem { Value = "A", Text = "Medication A" },
             new SelectListItem { Value = "B", Text = "Medication B" },
             new SelectListItem { Value = "C", Text = "Medication C" },
        };

        public string? EmbedUrl { get; set; }

        public string? JavascriptToRun { get; set; }
   
    }
}
