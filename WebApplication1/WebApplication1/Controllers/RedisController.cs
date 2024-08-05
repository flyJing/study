using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class RedisController: ControllerBase
{
    private readonly ConnectionMultiplexer _redis;
    
    public RedisController(ConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer;
    }

    [HttpGet, Route("hash")]
    public async Task<IActionResult> Hash()
    {
        var database = _redis.GetDatabase();
        database.HashSet("hash", "id", "234");
        
        var hashGetAll = database.HashGetAll("hash");
        foreach (var hashEntry in hashGetAll)
        {
            Console.WriteLine(hashEntry);
        }
        
        database.KeyDelete("hash");
        return Ok();
    }
    
    [HttpGet, Route("set")]
    public async Task<IActionResult> Set()
    {
        var database = _redis.GetDatabase();
        database.SetAdd("set", "barak");
        
        var redisValues = database.SetMembers("set");
        foreach (var redisValue in redisValues)
        {
            Console.WriteLine(redisValue);
        }
        
        database.SetAdd("set2", "barak");
        // 交集
        database.SetCombine(SetOperation.Union, "set", "set2");
        
        return Ok();
    }
    
    [HttpGet, Route("list")]
    public async Task<IActionResult> List()
    {
        var database = _redis.GetDatabase();

        RedisValue[] arr = new RedisValue[10];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i]=  new RedisValue("正确"+i);
        }
        long isf = database.ListRightPush("mylist",arr);

        var redisValues = database.ListRange("mylist");
        foreach (var redisValue in redisValues)
        {
            Console.WriteLine(redisValue);
        }

        return Ok();
    }
    
    [HttpGet, Route("zset")]
    public async Task<IActionResult> ZSet()
    {
        var database = _redis.GetDatabase();

        database.SortedSetAdd("zset", "barak", 10);
        var sortedSetRangeByScore = database.SortedSetRangeByScore("zset");
        foreach (var redisValue in sortedSetRangeByScore)
        {
            Console.WriteLine(redisValue);
        }
        return Ok();
    }
    
    [HttpGet, Route("error/test")]
    public async Task<IActionResult> ErrorTest()
    {
        var database = _redis.GetDatabase();
        var result = await database.ExecuteAsync("INVALID_COMMAND");
        return Ok();
    }
}