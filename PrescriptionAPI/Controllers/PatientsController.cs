using Microsoft.AspNetCore.Mvc;
using PrescriptionAPI.Services;
using System.Threading.Tasks;

namespace PrescriptionAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase {
        private readonly IPrescriptionService _service;
        public PatientsController(IPrescriptionService service) {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var result = await _service.GetPatientAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}