using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class HangFireController: ControllerBase
{
    [HttpGet, Route("simple/sendMsg")]
    public async Task<IActionResult> SimpleSendMsg()
    {

        return Ok();
    }

}

