using backend.Models;

namespace brazilian_universities_backend.Services
{
    public interface IUniversityService
    {
        Task<PaginatedResult<University>> GetUniversitiesAsync(int page, int pageSize);
    }
}