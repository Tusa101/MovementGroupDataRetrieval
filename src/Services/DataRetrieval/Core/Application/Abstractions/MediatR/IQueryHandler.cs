using MediatR;

namespace Application.Abstractions.MediatR;
public interface IQueryHandler<in TRequest, TResponse> :
    IRequestHandler<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{
}
