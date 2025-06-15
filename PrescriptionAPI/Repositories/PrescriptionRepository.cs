using System.Threading.Tasks;
using PrescriptionAPI.Data;
using PrescriptionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionAPI.Repositories {
    public class PrescriptionRepository : IPrescriptionRepository {
        private readonly PrescriptionDbContext _context;
        public PrescriptionRepository(PrescriptionDbContext context) {
            _context = context;
        }
        public async Task<Patient> GetPatientAsync(int id) {
            return await _context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .FirstOrDefaultAsync(p => p.IdPatient == id);
        }
        public async Task<Doctor> GetDoctorAsync(int id) {
            return await _context.Doctors.FindAsync(id);
        }
        public async Task<Medicament> GetMedicamentAsync(int id) {
            return await _context.Medicaments.FindAsync(id);
        }
        public async Task AddPrescriptionAsync(Prescription prescription) {
            await _context.Prescriptions.AddAsync(prescription);
        }
        public async Task SaveChangesAsync() {
            await _context.SaveChangesAsync();
        }
    }
}