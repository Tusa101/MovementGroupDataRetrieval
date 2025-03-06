using MediatR;

namespace Application.Abstractions.MediatR;
public interface ICommand<out T> : IRequest<T>
{
}
