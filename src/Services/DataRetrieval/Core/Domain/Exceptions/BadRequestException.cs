using Domain.CommonConstants;

namespace Domain.Exceptions;
public class BadRequestException(string message = "") : BaseException(message, ErrorCodes.BadRequestError)
{
}
