using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Mediator.Net;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Common;
using WebApplication1.Dto;
using WebApplication1.Message;
using WebApplication1.Message.Command;
using WebApplication1.Message.Request;
using WebApplication1.Message.Response;


namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;
    
    // GET
    public StudentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /**
     * 查询
     */
    [HttpGet,Route("get")]
    public async Task<IActionResult> Get([FromQuery] GetStudentListRequest request)
    {
        var result = await _mediator.RequestAsync<GetStudentListRequest,
            GetStudentListResponse>(request);
        return Ok(result);
    }
    
    /**
     * 删除
     */
    [HttpDelete,Route("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteStudentCommand command)
    {
        await _mediator.SendAsync(command);
        return Ok("删除成功");
    }
    
    /**
     * 添加
     */
    [HttpPost,Route("add")]
    public async Task<IActionResult> AddUser(AddStudentCommand command)
    {
        var result = await _mediator.SendAsync<AddStudentCommand, AddStudentResponse>(command);
        return Ok();
    }
    
    /**
     * 修改
     */
    [HttpPost,Route("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateStudentCommand command)
    {
        await _mediator.SendAsync(command);
        return Ok("修改成功");
    }
    
}