using Microsoft.EntityFrameworkCore;
using PrescriptionAPI.Models;

namespace PrescriptionAPI.Data {
    public class PrescriptionDbContext : DbContext {
        public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options) : base(options) { }
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Medicament> Medicaments => Set<Medicament>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments => Set<PrescriptionMedicament>();
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<PrescriptionMedicament>()
                .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });
        }
    }
}