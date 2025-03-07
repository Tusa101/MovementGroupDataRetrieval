using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Presentation.Controllers;

public class DataController : ApiController
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok();
    }
    
    [HttpPost]
    [Authorize(Roles = ApplicationRoles.Admin)]
    public IActionResult Post()
    {
        return Ok();
    }
    
    [HttpPut]
    [Authorize(Roles = ApplicationRoles.Admin)]
    public IActionResult Put()
    {
        return Ok();
    }
}
