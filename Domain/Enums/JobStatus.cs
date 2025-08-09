namespace Domain.Enums;

/// <summary>
/// Represents the various stages of a job application process.
/// Values are ordered roughly by typical progression through the hiring process.
/// </summary>
public enum JobStatus
{
    /// <summary>
    /// Application has been submitted but no response received
    /// </summary>
    Applied = 1,
    
    /// <summary>
    /// Initial screening or phone interview scheduled/completed
    /// </summary>
    Screening = 2,
    
    /// <summary>
    /// Technical or in-person interview scheduled/completed
    /// </summary>
    Interview = 3,
    
    /// <summary>
    /// Multiple rounds of interviews completed, awaiting decision
    /// </summary>
    FinalRound = 4,
    
    /// <summary>
    /// Job offer has been extended
    /// </summary>
    Offer = 5,
    
    /// <summary>
    /// Offer accepted, starting soon or already started
    /// </summary>
    Accepted = 6,
    
    /// <summary>
    /// Application was rejected at any stage
    /// </summary>
    Rejected = 7,
    
    /// <summary>
    /// Decided not to pursue this opportunity further
    /// </summary>
    Withdrawn = 8,
    
    /// <summary>
    /// No response received after significant time
    /// </summary>
    NoResponse = 9
}