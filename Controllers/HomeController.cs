using Microsoft.AspNetCore.Mvc;

namespace AIChatbot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Home Controller is working!");
    }
} 