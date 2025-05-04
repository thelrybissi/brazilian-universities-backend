using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using backend.Models;


namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversitiesController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public UniversitiesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var universities = await _httpClient.GetFromJsonAsync<List<University>>(
                "http://universities.hipolabs.com/search?country=Brazil"
            );

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