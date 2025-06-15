using System.Threading.Tasks;
using PrescriptionAPI.DTOs;

namespace PrescriptionAPI.Services {
    public interface IPrescriptionService {
        Task AddPrescriptionAsync(PrescriptionCreateDTO dto);
        Task<PatientResponseDto> GetPatientAsync(int id);
    }
}