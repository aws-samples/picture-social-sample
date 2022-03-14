using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloWorldController : ControllerBase
{

    [HttpGet("{name}")]
    public string Get(string name)
    {
        return $"Hello {name}";
    }
}
