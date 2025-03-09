using Domain.Exceptions;
using Shared.Constants;

namespace Application.Exceptions;
public sealed class ValidationException(Dictionary<string, string[]> errors)
    : BadRequestException(ErrorCodes.ValidationError)
{
    public Dictionary<string, string[]> Errors { get; } = errors;
}
