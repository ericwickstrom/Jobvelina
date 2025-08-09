using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Core Job entity representing a job application.
/// This is the heart of our domain model - contains all business rules and properties.
/// </summary>
public class Job
{
    /// <summary>
    /// Unique identifier for the job application
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Foreign key to the Company entity
    /// </summary>
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Navigation property to the Company entity
    /// </summary>
    public Company Company { get; set; } = null!;
    
    /// <summary>
    /// Job position/title applied for
    /// </summary>
    public string Position { get; set; } = string.Empty;
    
    /// <summary>
    /// Current status of the job application
    /// </summary>
    public JobStatus Status { get; set; }
    
    /// <summary>
    /// Date when the application was submitted
    /// </summary>
    public DateTime ApplicationDate { get; set; }
    
    /// <summary>
    /// When this record was created in the system
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// When this record was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Optional description or notes about the job/application
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Optional salary information
    /// </summary>
    public string? Salary { get; set; }
    
    /// <summary>
    /// Optional URL to the specific job posting
    /// </summary>
    public string? JobPostingUrl { get; set; }
    
    /// <summary>
    /// Domain method to update the job status
    /// This encapsulates business logic at the domain level
    /// </summary>
    public void UpdateStatus(JobStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Domain method to update job details
    /// Ensures UpdatedAt is always set when changes are made
    /// </summary>
    public void UpdateDetails(string position, string? description = null, 
                             string? salary = null, string? jobPostingUrl = null)
    {
        Position = position;
        Description = description;
        Salary = salary;
        JobPostingUrl = jobPostingUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}