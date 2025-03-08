using Application.Abstractions.MediatR;

namespace Application.Features.Data.Commands.AddStoredData;
public sealed record AddStoredDataCommand(string Content) : ICommand<AddStoredDataResponse>;
