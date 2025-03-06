using System.Transactions;
using Application.Abstractions.MediatR;
using Domain.Abstractions;
using MediatR;

namespace Application.Behaviors;
/// <summary>
/// This class is a behavior that wraps the execution of a request handler with a unit of work.
/// If the request is not a command, it will execute the next handler and save changes to the unit of work.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
/// <typeparam name="TResponse">The type of response returned by the request handler.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="UnitOfWorkBehavior{TRequest, TResponse}"/> class.
/// </remarks>
/// <param name="unitOfWork">The unit of work to be used by the behavior.</param>
public class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{

    /// <summary>
    /// Handles the request by executing the next handler and saving changes if the request is a command.
    /// </summary>
    /// <param name="request">The request to be handled.</param>
    /// <param name="next">The next handler in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response returned by the next handler.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (IsNotCommand())
        {
            return await next();
        }

        using var transactionScope = new TransactionScope();
        var response = await next();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        transactionScope.Complete();

        return response;
    }

    /// <summary>
    /// Determines whether the request is a command.
    /// </summary>
    /// <returns>True if the request is not a command, false otherwise.</returns>
    private static bool IsNotCommand()
    {
        return !typeof(TRequest).GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICommand<>));
    }
}