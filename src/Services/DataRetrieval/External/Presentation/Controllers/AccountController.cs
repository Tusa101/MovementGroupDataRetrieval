using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Presentation.Controllers;
public class AccountController : ApiController
{
    [HttpPost(HttpEndpoints.Account.RegisterUser)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        Mapper.Map();
        return Ok();
    }
}
