using Microsoft.AspNetCore.Mvc;
using Nest;
using WebApplication1.Entity.Elasticsearch;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class ElasticsearchController : ControllerBase
{
    private readonly IElasticClient _client;

    public ElasticsearchController(IElasticClient client)
    {
        _client = client;
    }


    [HttpGet, Route("get")]
    public async Task<IActionResult> Get()
    {
        var response = await _client.SearchAsync<Barak>(x => x.Index("barak"));
        Console.WriteLine(response);
        if (response.IsValid)
        {
            foreach (var hit in response.Hits)
            {
                var model = new Barak()
                {
                    Email = hit.Source.Email,
                    Name = hit.Source.Name,
                    Age = hit.Source.Age,
                };
                Console.WriteLine($"邮箱：{model.Email};\t姓名：{model.Name};\t年龄：{model.Age}");
            }
        }
        else
        {
            Console.WriteLine($"查询失败：{response.OriginalException.Message}");
        }

        return Ok();
    }

    [HttpPost, Route("Add")]
    public async Task<IActionResult> Add()
    {
        var barak = new Barak()
        {
            Name = "snake2",
            Email = "123qq.com",
            Age = 22
        };
        
        var response = await _client.IndexDocumentAsync(barak).ConfigureAwait(false);
        if (response.IsValid)
        {
            Console.WriteLine($"新增ID为{response.Id}的数据成功");
        }
        else
        {
            Console.WriteLine($"操作失败:{response.OriginalException.Message}");
        }

        return Ok();
    }
    
    [HttpPost, Route("update")]
    public async Task<IActionResult> Update()
    {
        var barak = new Barak()
        {
            Name = "snake3",
            Email = "123qq.com",
            Age = 22
        };

        await _client.UpdateAsync<Barak>("PJiuGY8Bt56zWO5owhIM", x => x.Doc(barak));
       

        return Ok();
    }
}