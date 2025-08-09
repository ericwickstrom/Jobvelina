using Domain.Enums;

namespace Application.DTOs;

/// <summary>
/// Data Transfer Object for Job entities.
/// Used to transfer job data between layers without exposing domain entities directly.
/// Matches the structure expected by your React frontend.
/// </summary>
public class JobDto
{
    public Guid Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public JobStatus Status { get; set; }
    public DateTime ApplicationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Description { get; set; }
    public string? Salary { get; set; }
    public string? JobPostingUrl { get; set; }
}

/// <summary>
/// DTO for creating new job applications.
/// Excludes system-generated fields like Id, CreatedAt, UpdatedAt.
/// </summary>
public class CreateJobDto
{
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public JobStatus Status { get; set; } = JobStatus.Applied;
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }
    public string? Salary { get; set; }
    public string? JobPostingUrl { get; set; }
}

/// <summary>
/// DTO for updating existing job applications.
/// Allows partial updates - all fields are optional except Id.
/// </summary>
public class UpdateJobDto
{
    public Guid Id { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public JobStatus? Status { get; set; }
    public DateTime? ApplicationDate { get; set; }
    public string? Description { get; set; }
    public string? Salary { get; set; }
    public string? JobPostingUrl { get; set; }
}