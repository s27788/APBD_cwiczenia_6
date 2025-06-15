using System.Collections.Generic;

namespace PrescriptionAPI.Models {
    public class Medicament {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new();
    }
}