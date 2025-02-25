using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public UserController()
    {
        
    }

    [HttpGet("test/{testVariable}")]
    public string[] Test(string testVariable)
    {
        return ["test", testVariable];
    }
}