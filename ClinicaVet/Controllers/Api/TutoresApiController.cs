using ClinicaVet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVet.Controllers.Api;

[ApiController]
[Route("api/tutores")]
[Authorize]
public class TutoresApiController : ControllerBase
{
    private readonly ClinicaContext _db;

    public TutoresApiController(ClinicaContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _db.Tutores
        .AsNoTracking()
        .OrderBy(t => t.Nome)
        .Select(t => new { t.Id, t.Nome, t.Email, t.Telefone })
        .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var tutor = await _db.Tutores.AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new { t.Id, t.Nome, t.Email, t.Telefone })
            .FirstOrDefaultAsync();

        return tutor is null ? NotFound() : Ok(tutor);
    }
}
