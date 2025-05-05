using backend.Models;

namespace brazilian_universities_backend.Services
{
    public interface IUniversityService
    {
        Task<List<University>?> GetUniversitiesAsync();
    }
}