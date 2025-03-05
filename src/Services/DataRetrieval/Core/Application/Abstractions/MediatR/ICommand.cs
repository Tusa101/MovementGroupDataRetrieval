using MediatR;

namespace Application.Abstractions.MediatR;
public interface ICommand<T> : IRequest<T>
{
}
