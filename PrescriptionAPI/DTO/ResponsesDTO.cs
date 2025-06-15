using System;
using System.Collections.Generic;

namespace PrescriptionAPI.DTOs {
    public class DoctorDto {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class MedicamentResponseDto {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public int Dose { get; set; }
        public string Description { get; set; }
    }

    public class PrescriptionResponseDto {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DoctorDto Doctor { get; set; }
        public List<MedicamentResponseDto> Medicaments { get; set; }
    }

    public class PatientResponseDto {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PrescriptionResponseDto> Prescriptions { get; set; }
    }
}