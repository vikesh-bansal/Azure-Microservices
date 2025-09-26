using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace WPM.Clinic.DataAccess
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
           
        }
        public DbSet<Consultation> Consultations { get; set; }

    }
    public record Consultation(Guid Id, int PatientId, string PatientName, int PatentAge, DateTime StartTime);
    public static class ClinicDbContextExtensions
    {
        public static void EnsureClinicDbIsCreate(this IApplicationBuilder app) { 
        
            using var scope =app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ClinicDbContext>();
            context!.Database.EnsureCreated();
        }
    }
}
