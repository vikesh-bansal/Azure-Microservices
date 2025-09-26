using WPM.Clinic.Controllers;
using WPM.Clinic.DataAccess;
using WPM.Clinic.ExternalServices;
using Microsoft.Extensions.Caching.Memory;
namespace WPM.Clinic.Application
{
    public class ClinicApplicationService
    {
        private readonly ClinicDbContext _dbContext;
        private readonly ManagementService _managementService;
        private readonly IMemoryCache _memoryCache;
        public ClinicApplicationService(ClinicDbContext dbContext, ManagementService managementService, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _managementService = managementService;
            _memoryCache = memoryCache;
        }
        public async Task<Consultation> Handle(StartConsultionCommand command)
        {
            PetInfo? petInfo = await _memoryCache.GetOrCreateAsync(command.PatientId, async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return await _managementService.GetPetInfo(command.PatientId);
            });
            var newConsultation = new Consultation(Guid.NewGuid(), command.PatientId, petInfo.Name, petInfo.Age, DateTime.UtcNow);
            await _dbContext.Consultations.AddAsync(newConsultation);
            await _dbContext.SaveChangesAsync();
            return newConsultation;
        }
    }
}
