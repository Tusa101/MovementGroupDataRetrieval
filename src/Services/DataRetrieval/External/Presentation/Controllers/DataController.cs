using System.ComponentModel.DataAnnotations;
using Application.Features.Data.Commands.AddStoredData;
using Application.Features.Data.Commands.UpdateStoredData;
using Application.Features.Data.Queries.GetStoredData;
using AutoMapper;
using Domain.Exceptions;
using Infrastructure.Configuration.Extensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Presentation.Controllers;

/// <summary>
/// The DataController class is responsible for handling data-related API requests.
/// </summary>
public class DataController : ApiController
{
    /// <summary>
    /// Retrieves a stored data item by its ID.
    /// </summary>
    /// <param name="id">The ID of the stored data item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved data item.</returns>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(GetStoredDataResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await Mediator.Send(new GetStoredDataQuery(id)));
    }

    /// <summary>
    /// Adds a new stored data item.
    /// </summary>
    /// <param name="content">The content of the stored data item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added data item.</returns>
    [HttpPost]
    [Authorize(Roles = ApplicationRoles.Admin)]
    [ProducesResponseType(typeof(AddStoredDataCommand), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] string content)
    {
        return Ok(await Mediator.Send(new AddStoredDataCommand(content)));
    }

    /// <summary>
    /// Updates an existing stored data item.
    /// </summary>
    /// <param name="id">The ID of the stored data item to update.</param>
    /// <param name="content">The updated content of the stored data item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated data item.</returns>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = ApplicationRoles.Admin)]
    [ProducesResponseType(typeof(UpdateStoredDataCommand), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Put(Guid id, [FromBody][Required] string content)
    {
        return Ok(await Mediator.Send(new UpdateStoredDataCommand(id, content)));
    }
}
