using Microsoft.AspNetCore.Mvc;
using WebApplication1.AzureServiceBus;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class AzureServiceBusController: ControllerBase
{
    [HttpGet, Route("testQueue")]
    public async Task<IActionResult> Queue(CancellationToken cancellationToken)
    {
        var simpleTest = new SimpleTest();
        await simpleTest.QueuePublish();
        await simpleTest.ConsumerQueueMessage();
        return Ok();
    }
    
    [HttpGet, Route("testTopic")]
    public async Task<IActionResult> Topic(CancellationToken cancellationToken)
    {
        var simpleTest = new SimpleTest();
        await simpleTest.TopicPublish();
        await simpleTest.ConsumerTopicMessage();
        return Ok();
    }
    
    [HttpGet, Route("delete/message")]
    public async Task<IActionResult> DeleteMessages(CancellationToken cancellationToken)
    {
        var simpleTest = new SimpleTest();
        await simpleTest.DeleteMessages();
        return Ok();
    }
}