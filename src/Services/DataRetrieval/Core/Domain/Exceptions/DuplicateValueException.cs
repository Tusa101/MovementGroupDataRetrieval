using Domain.CommonConstants;

namespace Domain.Exceptions;

public class DuplicateValueException : BaseException
{
    public DuplicateValueException(string message = "") : base(message, ErrorCodes.DuplicateValueError)
    {
    }

    public DuplicateValueException(string type, int id) : base($"{type} with {id} already exists", ErrorCodes.DuplicateValueError)
    {

    }

    public DuplicateValueException(string type, Guid id) : base($"{type} with {id} already exists", ErrorCodes.DuplicateValueError)
    {

    }
}
