using Application.Abstractions.MediatR;

namespace Application.Features.Data.Commands.AddStoredData;
public sealed record AddStoredDataCommand(Guid Id, string Content) : ICommand<AddStoredDataResponse>;
