using System.Threading.Tasks;
using PrescriptionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionAPI.Repositories {
    public interface IPrescriptionRepository {
        Task<Patient> GetPatientAsync(int id);
        Task<Doctor> GetDoctorAsync(int id);
        Task<Medicament> GetMedicamentAsync(int id);
        Task AddPrescriptionAsync(Prescription prescription);
        Task SaveChangesAsync();
    }
}