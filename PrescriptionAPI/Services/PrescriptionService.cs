using System;
using System.Linq;
using System.Threading.Tasks;
using PrescriptionAPI.DTOs;
using PrescriptionAPI.Models;
using PrescriptionAPI.Repositories;

namespace PrescriptionAPI.Services {
    public class PrescriptionService : IPrescriptionService {
        private readonly IPrescriptionRepository _repo;
        public PrescriptionService(IPrescriptionRepository repo) {
            _repo = repo;
        }

        public async Task AddPrescriptionAsync(PrescriptionCreateDTO dto) {
            if (dto.Medicaments.Count > 10)
                throw new ArgumentException("Prescription cannot have more than 10 medicaments");
            if (dto.DueDate < dto.Date)
                throw new ArgumentException("DueDate must be >= Date");

            // Patient
            Patient patient;
            if (dto.Patient.IdPatient.HasValue) {
                patient = await _repo.GetPatientAsync(dto.Patient.IdPatient.Value);
                if (patient == null)
                    throw new ArgumentException("Patient not found");
            } else {
                patient = new Patient {
                    FirstName = dto.Patient.FirstName,
                    LastName = dto.Patient.LastName
                };
            }

            // Doctor
            var doctor = await _repo.GetDoctorAsync(dto.DoctorId);
            if (doctor == null)
                throw new ArgumentException("Doctor not found");

            // Prepare prescription
            var prescription = new Prescription {
                Date = dto.Date,
                DueDate = dto.DueDate,
                Patient = patient,
                Doctor = doctor
            };

            foreach (var medDto in dto.Medicaments) {
                var med = await _repo.GetMedicamentAsync(medDto.IdMedicament);
                if (med == null)
                    throw new ArgumentException($"Medicament {medDto.IdMedicament} not found");

                prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament {
                    Prescription = prescription,
                    Medicament = med,
                    Dose = medDto.Dose,
                    Description = medDto.Description
                });
            }

            await _repo.AddPrescriptionAsync(prescription);
            await _repo.SaveChangesAsync();
        }

        public async Task<PatientResponseDto> GetPatientAsync(int id) {
            var patient = await _repo.GetPatientAsync(id);
            if (patient == null)
                return null;

            return new PatientResponseDto {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Prescriptions = patient.Prescriptions
                    .OrderBy(p => p.DueDate)
                    .Select(pr => new PrescriptionResponseDto {
                        IdPrescription = pr.IdPrescription,
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        Doctor = new DoctorDto {
                            IdDoctor = pr.Doctor.IdDoctor,
                            FirstName = pr.Doctor.FirstName,
                            LastName = pr.Doctor.LastName
                        },
                        Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentResponseDto {
                            IdMedicament = pm.IdMedicament,
                            Name = pm.Medicament.Name,
                            Dose = pm.Dose,
                            Description = pm.Description
                        }).ToList()
                    }).ToList()
            };
        }
    }
}