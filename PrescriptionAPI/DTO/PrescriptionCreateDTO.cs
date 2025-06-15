using System;
using System.Collections.Generic;

namespace PrescriptionAPI.DTOs {
    public class PatientDto {
        public int? IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class MedicamentDto {
        public int IdMedicament { get; set; }
        public int Dose { get; set; }
        public string Description { get; set; }
    }

    public class PrescriptionCreateDTO {
        public PatientDto Patient { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public List<MedicamentDto> Medicaments { get; set; }
    }
}