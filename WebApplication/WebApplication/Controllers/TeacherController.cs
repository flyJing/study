using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Mediator.Net;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Common;
using WebApplication.Dto;
using WebApplication.Message;
using WebApplication.Message.Command;
using WebApplication.Message.Request;
using WebApplication.Message.Response;


namespace WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class TeacherController : ControllerBase
{
    private readonly IMediator _mediator;
    
    // GET
    public TeacherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /**
     * 查询
     */
    [HttpGet,Route("get")]
    public async Task<IActionResult> Get([FromQuery] GetTeacherListRequest request)
    {
        var result = await _mediator.RequestAsync<GetTeacherListRequest,
            GetTeacherListResponse>(request);
        return Ok(result.TeacherDtoList);
    }
    
    /**
     * 删除
     */
    [HttpDelete,Route("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteTeacherCommand command)
    {
        await _mediator.SendAsync(command);
        return Ok();
    }
    
    /**
     * 添加
     */
    [HttpPost,Route("add")]
    public async Task<IActionResult> AddTeacher(AddTeacherCommand command)
    {
        var result = await _mediator.SendAsync<AddTeacherCommand, AddTeacherResponse>(command);
        return Ok(result.TeacherDto);
    }
    
    /**
     * 修改
     */
    [HttpPost,Route("update")]
    public async Task<IActionResult> UpdateTeacher([FromBody] UpdateTeacherCommand command)
    {
        await _mediator.SendAsync(command);
        return Ok();
    }
    
}