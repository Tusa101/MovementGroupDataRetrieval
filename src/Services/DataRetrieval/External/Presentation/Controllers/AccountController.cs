using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Commands.RevokeTokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
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

    [Authorize]
    [HttpPost(HttpEndpoints.Account.RevokeTokens)]
    [SwaggerOperation(Summary = "Revoke tokens")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeTokens()
    {
        var command = new RevokeTokensCommand(
            new Guid(HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub)!));

        return Ok(await Sender.Send(command));
    }
}
