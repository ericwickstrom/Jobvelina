using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Services
{
    public class MockJobService : IJobService
    {
        private readonly List<Job> _jobs;
        private readonly List<Company> _companies;
        private readonly object _lock = new object();

        public MockJobService()
        {
            // Initialize with some seed companies
            _companies = new List<Company>
            {
                new Company
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "TechCorp Solutions",
                    Website = "https://techcorp.com",
                    Industry = "Technology",
                    Size = "201-500 employees"
                },
                new Company
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "DataFlow Industries",
                    Website = "https://dataflow.io",
                    Industry = "Data Analytics",
                    Size = "51-200 employees"
                },
                new Company
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "StartupHub Inc",
                    Website = "https://startuphub.com",
                    Industry = "Startup Incubator",
                    Size = "11-50 employees"
                }
            };

            // Initialize with some seed job applications
            _jobs = new List<Job>
            {
                new Job
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    CompanyId = _companies[0].Id,
                    Company = _companies[0],
                    Position = "Senior Full Stack Developer",
                    Description = "Build modern web applications using React and .NET Core. Remote-friendly position in Seattle, WA.",
                    Salary = "$120,000 - $140,000",
                    JobPostingUrl = "https://techcorp.com/careers/senior-fullstack",
                    Status = JobStatus.Applied,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5),
                    ApplicationDate = DateTime.UtcNow.AddDays(-5)
                },
                new Job
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    CompanyId = _companies[1].Id,
                    Company = _companies[1],
                    Position = "Data Engineer",
                    Description = "Design and maintain data pipelines for analytics platform. Located in Austin, TX.",
                    Salary = "$100,000 - $130,000",
                    JobPostingUrl = "https://dataflow.io/jobs/data-engineer",
                    Status = JobStatus.Interview,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2),
                    ApplicationDate = DateTime.UtcNow.AddDays(-10)
                }
            };
        }

        public async Task<JobDto> CreateJobAsync(CreateJobDto createJobDto)
        {
            await Task.Delay(10); // Simulate async operation

            lock (_lock)
            {
                // Find or create company
                var company = _companies.FirstOrDefault(c => c.Name.Equals(createJobDto.Company, StringComparison.OrdinalIgnoreCase));
                if (company == null)
                {
                    company = new Company
                    {
                        Id = Guid.NewGuid(),
                        Name = createJobDto.Company,
                        Website = null, // No website in CreateJobDto
                        Industry = "Unknown",
                        Size = "Unknown"
                    };
                    _companies.Add(company);
                }

                // Create new job
                var job = new Job
                {
                    Id = Guid.NewGuid(),
                    CompanyId = company.Id,
                    Company = company,
                    Position = createJobDto.Position,
                    Description = createJobDto.Description,
                    Salary = createJobDto.Salary,
                    JobPostingUrl = createJobDto.JobPostingUrl,
                    Status = createJobDto.Status,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ApplicationDate = createJobDto.ApplicationDate
                };

                _jobs.Add(job);
                return MapToDto(job);
            }
        }

        public async Task<IEnumerable<JobDto>> GetAllJobsAsync()
        {
            await Task.Delay(10); // Simulate async operation

            lock (_lock)
            {
                return _jobs.Select(MapToDto).ToList();
            }
        }

        public async Task<JobDto?> GetJobByIdAsync(Guid id)
        {
            await Task.Delay(10); // Simulate async operation

            lock (_lock)
            {
                var job = _jobs.FirstOrDefault(j => j.Id == id);
                return job != null ? MapToDto(job) : null;
            }
        }

        public async Task<JobDto> UpdateJobAsync(Guid id, UpdateJobDto updateJobDto)
        {
            await Task.Delay(10); // Simulate async operation

            lock (_lock)
            {
                var job = _jobs.FirstOrDefault(j => j.Id == id);
                if (job == null)
                {
                    throw new ArgumentException($"Job with ID {id} not found");
                }

                // Find or create company if changed
                if (!job.Company.Name.Equals(updateJobDto.Company, StringComparison.OrdinalIgnoreCase))
                {
                    var company = _companies.FirstOrDefault(c => c.Name.Equals(updateJobDto.Company, StringComparison.OrdinalIgnoreCase));
                    if (company == null)
                    {
                        company = new Company
                        {
                            Id = Guid.NewGuid(),
                            Name = updateJobDto.Company,
                            Website = null, // No website in UpdateJobDto
                            Industry = "Unknown",
                            Size = "Unknown"
                        };
                        _companies.Add(company);
                    }
                    job.CompanyId = company.Id;
                    job.Company = company;
                }

                // Update job properties
                if (updateJobDto.Position != null) job.Position = updateJobDto.Position;
                if (updateJobDto.Description != null) job.Description = updateJobDto.Description;
                if (updateJobDto.Salary != null) job.Salary = updateJobDto.Salary;
                if (updateJobDto.JobPostingUrl != null) job.JobPostingUrl = updateJobDto.JobPostingUrl;
                if (updateJobDto.Status.HasValue) job.Status = updateJobDto.Status.Value;
                job.UpdatedAt = DateTime.UtcNow;

                if (updateJobDto.ApplicationDate.HasValue)
                {
                    job.ApplicationDate = updateJobDto.ApplicationDate.Value;
                }

                return MapToDto(job);
            }
        }

        public async Task<bool> DeleteJobAsync(Guid id)
        {
            await Task.Delay(10); // Simulate async operation

            lock (_lock)
            {
                var job = _jobs.FirstOrDefault(j => j.Id == id);
                if (job == null)
                {
                    return false;
                }

                _jobs.Remove(job);
                return true;
            }
        }

        private static JobDto MapToDto(Job job)
        {
            return new JobDto
            {
                Id = job.Id,
                Company = job.Company.Name,
                Position = job.Position,
                Description = job.Description,
                Salary = job.Salary,
                JobPostingUrl = job.JobPostingUrl,
                Status = job.Status,
                CreatedAt = job.CreatedAt,
                UpdatedAt = job.UpdatedAt,
                ApplicationDate = job.ApplicationDate
            };
        }
    }
}