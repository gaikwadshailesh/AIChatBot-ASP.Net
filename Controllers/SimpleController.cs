using Microsoft.AspNetCore.Mvc;

namespace AIChatbot.Controllers;

[ApiController]
[Route("[controller]")]
public class SimpleController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Simple controller working!");
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Simple test endpoint working!");
    }
} 