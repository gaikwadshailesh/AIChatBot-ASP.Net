using Microsoft.AspNetCore.Mvc;

namespace AIChatbot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new 
        { 
            message = "Debug endpoint working",
            routes = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Template,
            controllerName = ControllerContext.ActionDescriptor.ControllerName
        });
    }
} 