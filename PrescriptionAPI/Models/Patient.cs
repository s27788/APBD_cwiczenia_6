using System.Collections.Generic;

namespace PrescriptionAPI.Models {
    public class Patient {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Prescription> Prescriptions { get; set; } = new();
    }
}