namespace PrescriptionAPI.Models {
    public class PrescriptionMedicament {
        public int IdPrescription { get; set; }
        public Prescription Prescription { get; set; }
        public int IdMedicament { get; set; }
        public Medicament Medicament { get; set; }
        public int Dose { get; set; }
        public string Description { get; set; }
    }
}