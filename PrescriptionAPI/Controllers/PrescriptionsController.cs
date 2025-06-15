using Microsoft.AspNetCore.Mvc;
using PrescriptionAPI.DTOs;
using PrescriptionAPI.Services;
using System;
using System.Threading.Tasks;

namespace PrescriptionAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase {
        private readonly IPrescriptionService _service;
        public PrescriptionsController(IPrescriptionService service) {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PrescriptionCreateDTO dto) {
            try {
                await _service.AddPrescriptionAsync(dto);
                return Created("", null);
            } catch (ArgumentException ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}