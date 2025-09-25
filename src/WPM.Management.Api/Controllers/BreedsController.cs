using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;
using WPM.Management.Api.DataAccess;

namespace WPM.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BreedsController : ControllerBase
    {
        private ManagementDbContext _dbContext;
        private ILogger<BreedsController> _logger;

        public BreedsController(ManagementDbContext dbContext, ILogger<BreedsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var all = await _dbContext.Breeds.ToListAsync();
            return all != null ? Ok(all) : NotFound();
        }

        [HttpGet("{Id}", Name = nameof(GetBreedById))]
        public async Task<IActionResult> GetBreedById(int Id)
        {
            var breed = await _dbContext.Breeds.FindAsync(Id);
            return breed != null ? Ok(breed) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBreed(NewBreed newBreed)
        {
            try
            {
                var breed = newBreed.ToBreed();
                await _dbContext.Breeds.AddAsync(breed);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBreedById), new { Id = breed.Id }, newBreed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
    }
    public record NewBreed(string name)
    {
        public Breed ToBreed()
        {
            return new Breed(0, name);
        }
    }
}
