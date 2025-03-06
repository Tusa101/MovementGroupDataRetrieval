using Shared.Constants;

namespace Domain.Exceptions;

public abstract class BaseException(string message = "", string code = ErrorCodes.UndefinedError)
    : Exception($"{code}:{message}")
{
    public string Code { get; set; } = code;
}
