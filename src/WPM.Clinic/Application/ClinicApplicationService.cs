using WPM.Clinic.Controllers;
using WPM.Clinic.DataAccess;
using WPM.Clinic.ExternalServices;

namespace WPM.Clinic.Application
{
    public class ClinicApplicationService
    {
        private ClinicDbContext _dbContext;
        private ManagementService _managementService;
        public ClinicApplicationService(ClinicDbContext dbContext, ManagementService managementService)
        {
            _dbContext = dbContext;
            _managementService = managementService;
        }
        public async Task<Consultation> Handle(StartConsultionCommand command)
        {
            var petInfo = await _managementService.GetPetInfo(command.PatientId);
            var newConsultation = new Consultation(Guid.NewGuid(), command.PatientId, petInfo.Name, petInfo.Age, DateTime.UtcNow);
            await _dbContext.Consultations.AddAsync(newConsultation);
            await _dbContext.SaveChangesAsync();
            return newConsultation;
        }
    }
}
