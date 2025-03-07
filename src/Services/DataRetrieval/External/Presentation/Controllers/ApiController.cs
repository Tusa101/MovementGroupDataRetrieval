using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Controllers;

/// <summary>
/// Base class for API controllers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    private IMapper _mapper;

    /// <summary>
    /// Gets the mapper instance. If not already initialized, it initializes it using the
    /// HttpContext's RequestServices.
    /// </summary>
    protected IMapper Mapper => 
        _mapper ??= HttpContext.RequestServices.GetService<IMapper>()!;

    private ISender _sender;

    /// <summary>
    /// Gets the MediatR sender instance. If not already initialized, it initializes it using the
    /// HttpContext's RequestServices.
    /// </summary>
    protected ISender Sender =>
        _sender ??= HttpContext.RequestServices.GetService<ISender>()!;
}
