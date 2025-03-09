using MediatR;

namespace Application.Abstractions.MediatR;
/// <summary>
/// Represents a query that returns a result of type T.
/// Implements the <see cref="IRequest{T}"/> interface.
/// </summary>
/// <typeparam name="T">The type of the result.</typeparam>
public interface IQuery<out T> : IRequest<T>
{
}
