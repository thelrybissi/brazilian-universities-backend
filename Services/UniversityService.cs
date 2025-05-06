using backend.Models;
using brazilian_universities_backend.Services;

public class UniversityService : IUniversityService
{
    private readonly HttpClient _httpClient;

    public UniversityService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<PaginatedResult<University>> GetUniversitiesAsync(int page, int pageSize)
    {
        var universities = await _httpClient.GetFromJsonAsync<List<University>>(
            "http://universities.hipolabs.com/search?country=Brazil"
        );

        if (universities == null)
        {
            return new PaginatedResult<University>(new List<University>(), 0, page, pageSize);
        }

        var totalCount = universities.Count;

        var items = universities
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedResult<University>(items, totalCount, page, pageSize);
    }
}
