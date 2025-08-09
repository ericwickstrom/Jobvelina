// Application/Interfaces/IJobService.cs
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IJobService
    {
        Task<JobDto> CreateJobAsync(CreateJobDto job);
        Task<IEnumerable<JobDto>> GetAllJobsAsync();
        Task<JobDto?> GetJobByIdAsync(Guid id);
        Task<JobDto> UpdateJobAsync(Guid id, UpdateJobDto job);
        Task<bool> DeleteJobAsync(Guid id);
    }
}