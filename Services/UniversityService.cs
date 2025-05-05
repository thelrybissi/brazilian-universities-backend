using backend.Models;

namespace brazilian_universities_backend.Services
{
    public class UniversityService : IUniversityService
    {

        private readonly HttpClient _httpClient;

        public UniversityService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<University>?> GetUniversitiesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<University>>(
                "http://universities.hipolabs.com/search?country=Brazil"
            );
        }
    }
}
