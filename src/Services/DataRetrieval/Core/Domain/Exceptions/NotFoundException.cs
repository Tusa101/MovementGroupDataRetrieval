using Shared.Constants;

namespace Domain.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message = "") : base(message, ErrorCodes.NotFoundError)
    {
    }

    public NotFoundException(string type, Guid id) : base($"{type} with {id} doesn't exist", ErrorCodes.NotFoundError)
    {

    }
}
