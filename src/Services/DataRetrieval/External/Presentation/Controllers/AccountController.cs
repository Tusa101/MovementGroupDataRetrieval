using System.Security.Claims;
using Application.Features.Users.Commands.LoginByRefresh;
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Commands.RevokeTokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;
public class AccountController : ApiController
{
    [AllowAnonymous]
    [HttpPost(HttpEndpoints.Account.RegisterUser)]
    [SwaggerOperation(Summary = "Register user")]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
		var command = Mapper.Map<RegisterUserCommand>(request);
		
        return Ok(await Sender.Send(command));
    }

    [AllowAnonymous]
    [HttpPost(HttpEndpoints.Account.LoginUser)]
    [SwaggerOperation(Summary = "Login user")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var command = Mapper.Map<LoginUserCommand>(request);

        return Ok(await Sender.Send(command));
    }

    [AllowAnonymous]
    [HttpPost(HttpEndpoints.Account.RefreshToken)]
    [SwaggerOperation(Summary = "Refresh token")]
    [ProducesResponseType(typeof(LoginByRefreshResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] LoginByRefreshRequest request)
    {
        var command = Mapper.Map<LoginByRefreshCommand>(request);

        return Ok(await Sender.Send(command));
    }

    [Authorize]
    [HttpPost(HttpEndpoints.Account.RevokeTokens)]
    [SwaggerOperation(Summary = "Revoke tokens")]
    [ProducesResponseType(typeof(RevokeTokensResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeTokens()
    {
        Guid? id = Guid.TryParse(HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var parsed)
            ? parsed : null;


        return id is null? 
            BadRequest() : 
            Ok(await Sender.Send(
                new RevokeTokensCommand((Guid)id)
            ));
    }
}
