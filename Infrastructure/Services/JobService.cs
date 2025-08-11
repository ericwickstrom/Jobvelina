using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class JobService : IJobService
{
    private readonly JobvelinaDbContext _context;

    public JobService(JobvelinaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobDto>> GetAllJobsAsync()
    {
        var jobs = await _context.Jobs
            .Include(j => j.Company)
            .ToListAsync();
        return jobs.Select(MapToDto);
    }

    public async Task<JobDto?> GetJobByIdAsync(Guid id)
    {
        var job = await _context.Jobs.FindAsync(id);
        return job != null ? MapToDto(job) : null;
    }

    public async Task<JobDto> CreateJobAsync(CreateJobDto createJobDto)
    {
        // First, find or create the company
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Name == createJobDto.Company);

        if (company == null)
        {
            company = new Company
            {
                Id = Guid.NewGuid(),
                Name = createJobDto.Company,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Position = createJobDto.Position,
            CompanyId = company.Id,
            Status = createJobDto.Status,
            ApplicationDate = createJobDto.ApplicationDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        return MapToDto(job);
    }

    public async Task<JobDto?> UpdateJobAsync(Guid id, UpdateJobDto updateJobDto)
    {
        var job = await _context.Jobs
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == id);
        
        if (job == null)
            return null;

        // Handle company change if needed
        if (job.Company.Name != updateJobDto.Company)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Name == updateJobDto.Company);

            if (company == null)
            {
                company = new Company
                {
                    Id = Guid.NewGuid(),
                    Name = updateJobDto.Company,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
            }

            job.CompanyId = company.Id;
        }

        job.Position = updateJobDto.Position;
        job.Status = updateJobDto.Status;
        job.ApplicationDate = updateJobDto.ApplicationDate;
        job.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(job);
    }

    public async Task<bool> DeleteJobAsync(Guid id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null)
            return false;

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<JobDto>> GetJobsByStatusAsync(JobStatus status)
    {
        var jobs = await _context.Jobs
            .Where(j => j.Status == status)
            .ToListAsync();

        return jobs.Select(MapToDto);
    }

    private static JobDto MapToDto(Job job)
    {
        return new JobDto
        {
            Id = job.Id,
            Position = job.Position,
            Company = job.Company.Name,
            Status = job.Status,
            ApplicationDate = job.ApplicationDate,
            CreatedAt = job.CreatedAt,
            UpdatedAt = job.UpdatedAt
        };
    }
}