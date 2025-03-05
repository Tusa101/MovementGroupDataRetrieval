using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Configuration.Extensions.Exceptions;
/// <summary>
/// Represents an extended problem details object.
/// </summary>
public sealed class ExtendedProblemDetails : ProblemDetails
{
    /// <summary>
    /// Gets or sets an array of error messages.
    /// </summary>
    /// <value>
    /// An array of ApiError objects representing the error messages.
    /// </value>
    public ApiError[]? Errors { get; set; }
}
