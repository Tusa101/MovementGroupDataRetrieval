using Application.Abstractions.MediatR;

namespace Application.Features.Data.Commands.UpdateStoredData;
public sealed record UpdateStoredDataCommand(Guid Id, string Content) : ICommand<UpdateStoredDataResponse>;
