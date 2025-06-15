using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrescriptionAPI.Data;
using PrescriptionAPI.DTOs;
using PrescriptionAPI.Models;
using PrescriptionAPI.Repositories;
using PrescriptionAPI.Services;
using Xunit;

namespace PrescriptionAPI.Tests {
    public class PrescriptionServiceTests {
        private readonly DbContextOptions<PrescriptionDbContext> _options;

        public PrescriptionServiceTests() {
            _options = new DbContextOptionsBuilder<PrescriptionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new PrescriptionDbContext(_options);
            context.Medicaments.Add(new Medicament { IdMedicament = 1, Name = "TestMed", Description = "desc", Type = "t" });
            context.Doctors.Add(new Doctor { IdDoctor = 1, FirstName = "Doc", LastName = "Test" });
            context.SaveChanges();
        }

        [Fact]
        public async Task AddPrescription_Success() {
            using var context = new PrescriptionDbContext(_options);
            var repo = new PrescriptionRepository(context);
            var service = new PrescriptionService(repo);

            var dto = new PrescriptionCreateDTO {
                Patient = new PatientDto { FirstName = "Testowicz", LastName = "Testowy" },
                DoctorId = 1,
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1),
                Medicaments = new System.Collections.Generic.List<MedicamentDto> {
                    new MedicamentDto { IdMedicament = 1, Dose = 2, Description = "desc" }
                }
            };

            await service.AddPrescriptionAsync(dto);
            var patient = await repo.GetPatientAsync(1);
            Assert.NotNull(patient);
            Assert.Single(patient.Prescriptions);
        }

        [Fact]
        public async Task AddPrescription_MoreThanTenMedicaments_Throws() {
            using var context = new PrescriptionDbContext(_options);
            var repo = new PrescriptionRepository(context);
            var service = new PrescriptionService(repo);

            var medList = Enumerable.Range(1, 11).Select(i => new MedicamentDto { IdMedicament = 1, Dose = 1, Description = "d" }).ToList();
            var dto = new PrescriptionCreateDTO {
                Patient = new PatientDto { FirstName = "Kasia", LastName = "Kowalska" },
                DoctorId = 1,
                Date = DateTime.Now,
                DueDate = DateTime.Now,
                Medicaments = medList
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddPrescriptionAsync(dto));
        }

        [Fact]
        public async Task AddPrescription_NonExistingMed_Throws() {
            using var context = new PrescriptionDbContext(_options);
            var repo = new PrescriptionRepository(context);
            var service = new PrescriptionService(repo);

            var dto = new PrescriptionCreateDTO {
                Patient = new PatientDto { FirstName = "Marek", LastName = "Kasiak" },
                DoctorId = 1,
                Date = DateTime.Now,
                DueDate = DateTime.Now,
                Medicaments = new System.Collections.Generic.List<MedicamentDto> {
                    new MedicamentDto { IdMedicament = 999, Dose = 1, Description = "desc test" }
                }
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddPrescriptionAsync(dto));
        }

        [Fact]
        public async Task GetPatient_ReturnsSortedPrescriptions() {
            using var context = new PrescriptionDbContext(_options);
            var repo = new PrescriptionRepository(context);
            var service = new PrescriptionService(repo);

            var patient = new Patient { FirstName = "Pawel", LastName = "Wojak" };
            context.Patients.Add(patient);
            var doctor = await context.Doctors.FirstAsync();
            var med = await context.Medicaments.FirstAsync();
            var p1 = new Prescription {
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2),
                Patient = patient,
                Doctor = doctor
            };
            p1.PrescriptionMedicaments.Add(new PrescriptionMedicament {
                Medicament = med,
                Dose = 1,
                Description = "desc test"
            });
            var p2 = new Prescription {
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1),
                Patient = patient,
                Doctor = doctor
            };
            p2.PrescriptionMedicaments.Add(new PrescriptionMedicament {
                Medicament = med,
                Dose = 1,
                Description = "desc test"
            });
            context.Prescriptions.AddRange(p1, p2);
            context.SaveChanges();

            var result = await service.GetPatientAsync(patient.IdPatient);
            Assert.Equal(2, result.Prescriptions.Count);
            Assert.True(result.Prescriptions[0].DueDate <= result.Prescriptions[1].DueDate);
        }
    }
}