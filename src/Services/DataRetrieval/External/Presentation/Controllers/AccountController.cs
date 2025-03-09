using System.Security.Claims;
using Application.Features.Users.Commands.LoginByRefresh;
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Commands.RevokeTokens;
using Infrastructure.Configuration.Extensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;
/// <summary>
/// The AccountController class is responsible for handling account-related API requests.
/// </summary>
public class AccountController : ApiController
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The registration request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [AllowAnonymous]
    [HttpPost(HttpEndpoints.Account.RegisterUser)]
    [SwaggerOperation(Summary = "Register user")]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var command = Mapper.Map<RegisterUserCommand>(request);

        return Ok(await Mediator.Send(command));
    }

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="request">The login request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [AllowAnonymous]
    [HttpPost(HttpEndpoints.Account.LoginUser)]
    [SwaggerOperation(Summary = "Login user")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var command = Mapper.Map<LoginUserCommand>(request);

        return Ok(await Mediator.Send(command));
    }

    /// <summary>
    /// Refreshes a user's token.
    /// </summary>
    /// <param name="request">The refresh token request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [AllowAnonymous]
    [HttpPost(HttpEndpoints.Account.RefreshToken)]
    [SwaggerOperation(Summary = "Refresh token")]
    [ProducesResponseType(typeof(LoginByRefreshResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExtendedProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefreshToken([FromBody] LoginByRefreshRequest request)
    {
        var command = Mapper.Map<LoginByRefreshCommand>(request);

        return Ok(await Mediator.Send(command));
    }

    /// <summary>
    /// Revokes a user's tokens.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Authorize]
    [HttpPost(HttpEndpoints.Account.RevokeTokens)]
    [SwaggerOperation(Summary = "Revoke tokens")]
    [ProducesResponseType(typeof(RevokeTokensResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeTokens()
    {
        Guid? id = Guid.TryParse(HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var parsed)
            ? parsed : null;

        return id is null ?
            BadRequest() :
            Ok(await Mediator.Send(
                new RevokeTokensCommand((Guid)id)
            ));
    }
}
