using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace Embedded_Signatures.Models
{
    public class PrescriptionViewModel
    {
        public string PatientName { get; set; }
        public string MedicationName { get; set; }

        public List<SelectListItem>? MedicationOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "med_A", Text = "Medication A" },
            new SelectListItem { Value = "med_B", Text = "Medication B" },
            new SelectListItem { Value = "med_C", Text = "Medication C"  },
        };
    }
}
