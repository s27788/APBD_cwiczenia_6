using System;
using System.Collections.Generic;

namespace PrescriptionAPI.Models {
    public class Prescription {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int IdPatient { get; set; }
        public Patient Patient { get; set; }
        public int IdDoctor { get; set; }
        public Doctor Doctor { get; set; }
        public List<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new();
    }
}