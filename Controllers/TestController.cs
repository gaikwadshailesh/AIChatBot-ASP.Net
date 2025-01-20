namespace AIChatbot.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Test Controller is working!");
    }

    [HttpGet("hello")]
    public IActionResult Hello()
    {
        return Ok("Hello from Test Controller!");
    }
} 