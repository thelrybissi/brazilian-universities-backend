using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using backend.Models;
using brazilian_universities_backend.Services;


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
    public async Task<IActionResult> Get()
    {
        try
        {
            
            var universities = await _universityService.GetUniversitiesAsync();

            if (universities == null || universities.Count == 0)
                return NotFound("Nenhuma universidade encontrada.");

            return Ok(universities);
        }
        catch (Exception)
        {
            return StatusCode(503, "Erro ao acessar o serviço de universidades.");
        }
    }
}