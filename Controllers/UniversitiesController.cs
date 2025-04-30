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
        var universities = await _httpClient.GetFromJsonAsync<List<University>>("http://universities.hipolabs.com/search?country=Brazil");
        return Ok(universities);
    }
}