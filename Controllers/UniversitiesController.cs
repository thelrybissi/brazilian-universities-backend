using Microsoft.AspNetCore.Mvc;
using brazilian_universities_backend.Services;
using backend.Models;


namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversitiesController : ControllerBase
{
    private readonly IUniversityService _universityService;

    public UniversitiesController(IUniversityService universityService)
    {
        _universityService = universityService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {

            if (page <= 0 || pageSize <= 0)
                return BadRequest("Parâmetros de paginação inválidos.");

            var result = await _universityService.GetUniversitiesAsync(page, pageSize);

            if (result.TotalCount == 0)
                return NotFound("Nenhuma universidade encontrada.");

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(503, "Erro ao acessar o serviço de universidades.");
        }
    }
}