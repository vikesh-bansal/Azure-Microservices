using Microsoft.AspNetCore.Mvc;
using WPM.Clinic.Application;

namespace WPM.Clinic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationController : ControllerBase
    {
        private ClinicApplicationService _clinicalApplicationService;
        public ConsultationController(ClinicApplicationService clinicApplicationService)
        {
            _clinicalApplicationService = clinicApplicationService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start(StartConsultionCommand command)
        {
            var result = await _clinicalApplicationService.Handle(command);
            return Ok(result);
        }
        
    }
    public record StartConsultionCommand(int PatientId);
}
