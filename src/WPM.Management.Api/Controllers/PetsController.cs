using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WPM.Management.Api.DataAccess;

namespace WPM.Management.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private ManagementDbContext _dbContext;
    public PetsController(ManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var all = await _dbContext.Pets.Include(p => p.Breed).ToListAsync();
        return Ok(all);
    }
    [HttpGet("{id}", Name = nameof(GetById))]
    public async Task<IActionResult> GetById(int id)
    {
        var pet = await _dbContext.Pets.Include(p => p.Breed).Where(p => p.Id == id).FirstOrDefaultAsync();

        return Ok(pet);
    }
    [HttpPost]
    public async Task<IActionResult> Create(NewPet newPet)
    {
        var pet = newPet.ToPet();
        await _dbContext.Pets.AddAsync(pet);
        await _dbContext.SaveChangesAsync();
        return CreatedAtRoute(nameof(GetById), new { id = pet.Id }, newPet);
    }
}

public record NewPet(string Name, int Age, int BreedId)
{
    public Pet ToPet()
    {
        return new Pet { Name = Name, Age = Age, BreedId = BreedId };
    }
}