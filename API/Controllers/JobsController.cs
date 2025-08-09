using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// REST API controller for managing job applications.
    /// Provides CRUD operations following RESTful conventions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IJobService jobService, ILogger<JobsController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        /// <summary>
        /// Get all job applications
        /// </summary>
        /// <returns>List of all job applications</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JobDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetAllJobs()
        {
            try
            {
                _logger.LogInformation("Retrieving all job applications");
                var jobs = await _jobService.GetAllJobsAsync();
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all job applications");
                return StatusCode(500, "An error occurred while retrieving job applications");
            }
        }

        /// <summary>
        /// Get a specific job application by ID
        /// </summary>
        /// <param name="id">The unique identifier of the job application</param>
        /// <returns>The job application if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobDto>> GetJobById(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving job application with ID: {JobId}", id);
                var job = await _jobService.GetJobByIdAsync(id);
                
                if (job == null)
                {
                    _logger.LogWarning("Job application with ID {JobId} not found", id);
                    return NotFound($"Job application with ID {id} not found");
                }

                return Ok(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving job application with ID: {JobId}", id);
                return StatusCode(500, "An error occurred while retrieving the job application");
            }
        }

        /// <summary>
        /// Create a new job application
        /// </summary>
        /// <param name="createJobDto">The job application data</param>
        /// <returns>The created job application</returns>
        [HttpPost]
        [ProducesResponseType(typeof(JobDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JobDto>> CreateJob([FromBody] CreateJobDto createJobDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for job creation: {@ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating new job application for company: {Company}, position: {Position}", 
                    createJobDto.Company, createJobDto.Position);

                var createdJob = await _jobService.CreateJobAsync(createJobDto);
                
                return CreatedAtAction(
                    nameof(GetJobById), 
                    new { id = createdJob.Id }, 
                    createdJob);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating job application for company: {Company}", createJobDto.Company);
                return StatusCode(500, "An error occurred while creating the job application");
            }
        }

        /// <summary>
        /// Update an existing job application
        /// </summary>
        /// <param name="id">The unique identifier of the job application</param>
        /// <param name="updateJobDto">The updated job application data</param>
        /// <returns>The updated job application</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobDto>> UpdateJob(Guid id, [FromBody] UpdateJobDto updateJobDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for job update: {@ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                // Ensure the ID in the URL matches the DTO
                updateJobDto.Id = id;

                _logger.LogInformation("Updating job application with ID: {JobId}", id);

                var updatedJob = await _jobService.UpdateJobAsync(id, updateJobDto);
                return Ok(updatedJob);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Job application with ID {JobId} not found for update", id);
                return NotFound($"Job application with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating job application with ID: {JobId}", id);
                return StatusCode(500, "An error occurred while updating the job application");
            }
        }

        /// <summary>
        /// Delete a job application
        /// </summary>
        /// <param name="id">The unique identifier of the job application</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteJob(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting job application with ID: {JobId}", id);

                var deleted = await _jobService.DeleteJobAsync(id);
                
                if (!deleted)
                {
                    _logger.LogWarning("Job application with ID {JobId} not found for deletion", id);
                    return NotFound($"Job application with ID {id} not found");
                }

                _logger.LogInformation("Successfully deleted job application with ID: {JobId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting job application with ID: {JobId}", id);
                return StatusCode(500, "An error occurred while deleting the job application");
            }
        }

        /// <summary>
        /// Get job applications by status
        /// </summary>
        /// <param name="status">The job application status to filter by</param>
        /// <returns>List of job applications with the specified status</returns>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<JobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetJobsByStatus(string status)
        {
            try
            {
                // Parse the status string to enum
                if (!Enum.TryParse<Domain.Enums.JobStatus>(status, true, out var jobStatus))
                {
                    _logger.LogWarning("Invalid job status provided: {Status}", status);
                    return BadRequest($"Invalid job status: {status}");
                }

                _logger.LogInformation("Retrieving job applications with status: {Status}", jobStatus);

                var allJobs = await _jobService.GetAllJobsAsync();
                var filteredJobs = allJobs.Where(j => j.Status == jobStatus);

                return Ok(filteredJobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving job applications by status: {Status}", status);
                return StatusCode(500, "An error occurred while retrieving job applications by status");
            }
        }
    }
}