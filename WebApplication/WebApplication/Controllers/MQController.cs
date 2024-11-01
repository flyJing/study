using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class MQController : ControllerBase
{
    private readonly IConnection _mq;

    public MQController(IConnection connection)
    {
        _mq = connection;
    }

    [HttpGet, Route("simple/sendMsg")]
    public async Task<IActionResult> SimpleSendMsg()
    {
        var channel = _mq.CreateModel();

        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

        //5. 构建byte消息数据包
        string message = "Hello2 RabbitMQ!";
        var body = Encoding.UTF8.GetBytes(message);
        //6. 发送数据包
        channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
        Console.WriteLine(" [x] Sent {0}", message);
        return Ok();
    }

    [HttpGet, Route("simple/consume")]
    public async Task<IActionResult> SimpleConsume()
    {
        var channel = _mq.CreateModel();
        
        channel.QueueDeclare(queue: "hello", durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        var message = "";
        consumer.Received += (model, ea) =>
        {
            message = Encoding.UTF8.GetString(ea.Body.Span);
            Console.WriteLine(" [x] Received {0}", message);
        };
        channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
        return Ok(message);
    }

    [HttpGet, Route("exchange/fanout/sendMsg")]
    public async Task<IActionResult> ExchangeFanoutSendMsg()
    {
        var channel = _mq.CreateModel();
        
        //声明交换机
        channel.ExchangeDeclare("fanout_exchange", ExchangeType.Fanout);
        // 声明队列, 注意这队列不能重复，如果是一个重复队列，那么就会成为worker模式
        channel.QueueDeclare(queue: "fanout_queue", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueBind("fanout_queue", "fanout_exchange", "", null);

        string message = "你好，订阅主题模式";
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish("fanout_exchange", "", null, body);
        Console.WriteLine(" [x] Sent {0}", message);
        return Ok();
    }
    
    [HttpGet, Route("exchange/fanout/consume")]
    public async Task<IActionResult> ExchangeFanoutConsume()
    {
        var channel = _mq.CreateModel();
        
        var message = "";
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            message = Encoding.UTF8.GetString(ea.Body.Span);
            Console.WriteLine(" [x] Received {0}", message);
        };
        channel.BasicConsume("fanout_queue", true, consumer);

        return Ok(message);
    }
    
    [HttpGet, Route("exchange/direct/sendMsg")]
    public async Task<IActionResult> ExchangeDirectSendMsg()
    {
        var channel = _mq.CreateModel();
        
        //声明交换机
        channel.ExchangeDeclare("direct_exchange", ExchangeType.Direct);
        
        channel.QueueDeclare(queue: "direct_queue1", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueDeclare(queue: "direct_queue2", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        
        channel.QueueBind("direct_queue1", "direct_exchange", "error", null);
        channel.QueueBind("direct_queue2", "direct_exchange", "info", null);

        string message = "你好，路由模式,只投递到error的key上";
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish("direct_exchange", "error", null, body);
        Console.WriteLine(" [x] Sent {0}", message);
        return Ok();
    }
    
    [HttpGet, Route("exchange/topic/sendMsg")]
    public async Task<IActionResult> ExchangeTopicSendMsg()
    {
        var channel = _mq.CreateModel();
        
        //声明交换机
        channel.ExchangeDeclare("topic_exchange", ExchangeType.Topic);
        
        channel.QueueDeclare(queue: "topic_queue1", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        channel.QueueDeclare(queue: "topic_queue2", durable: true, exclusive: false, autoDelete: false,
            arguments: null);
        
        channel.QueueBind("topic_queue1", "topic_exchange", "*.error", null);
        channel.QueueBind("topic_queue2", "topic_exchange", "#.info", null);

        string message = "你好，主题模式";
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish("direct_exchange", "test.error", null, body);
        Console.WriteLine(" [x] Sent {0}", message);
        return Ok();
    }
}