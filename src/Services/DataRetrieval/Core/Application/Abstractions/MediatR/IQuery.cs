using MediatR;

namespace Application.Abstractions.MediatR;
public interface IQuery<T> : IRequest<T>
{
}
