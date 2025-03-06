using Shared.Constants;

namespace Domain.Exceptions;
public class BadRequestException(string message = "") : BaseException(message, ErrorCodes.BadRequestError)
{
}
