using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ContextConfiguration;
using WebApplication1.Cosmos.Entity;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class CosmosController: ControllerBase
{
    private readonly CosmosDbContext _cosmosDbContext;

    public CosmosController(CosmosDbContext cosmosDbContext)
    {
        _cosmosDbContext = cosmosDbContext;
    }

    [HttpGet, Route("get")]
    public async Task<IActionResult> Get(string name, CancellationToken cancellationToken)
    {
        var cosmosUsers = await _cosmosDbContext.MyEntities.Where(x => x.Name.Contains(name))
            .ToListAsync(cancellationToken).ConfigureAwait(false);
        return Ok(cosmosUsers);
    }
    
    [HttpPost, Route("add")]
    public async Task<IActionResult> Add()
    {
        _cosmosDbContext.MyEntities.Add(new CosmosUser()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "barak"
        });

        await _cosmosDbContext.SaveChangesAsync();
        return Ok();
    }
}