namespace Domain.Entities;

/// <summary>
/// Company entity representing organizations where job applications are submitted.
/// Allows for rich company data and reusability across multiple job applications.
/// </summary>
public class Company
{
    /// <summary>
    /// Unique identifier for the company
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Company name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Company website URL
    /// </summary>
    public string? Website { get; set; }
    
    /// <summary>
    /// Company description or notes
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Company location/headquarters
    /// </summary>
    public string? Location { get; set; }
    
    /// <summary>
    /// Company size (e.g., "50-200 employees", "Fortune 500", etc.)
    /// </summary>
    public string? Size { get; set; }
    
    /// <summary>
    /// Industry or sector
    /// </summary>
    public string? Industry { get; set; }
    
    /// <summary>
    /// When this company record was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// When this company record was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Navigation property - all job applications to this company
    /// </summary>
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    
    /// <summary>
    /// Domain method to update company details
    /// </summary>
    public void UpdateDetails(string name, string? website = null, string? description = null,
                             string? location = null, string? size = null, string? industry = null)
    {
        Name = name;
        Website = website;
        Description = description;
        Location = location;
        Size = size;
        Industry = industry;
        UpdatedAt = DateTime.UtcNow;
    }
}