using Autofac;
using AutoMapper;
using Mediator.Net;
using Mediator.Net.Autofac;
using Mediator.Net.Binding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Shouldly;
using WebApplication1.Common;
using WebApplication1.ContextConfiguration;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Handler;
using WebApplication1.Message;
using WebApplication1.Message.Command;
using WebApplication1.Message.Request;
using WebApplication1.Message.Response;

namespace TestProject1;

public class StudentUnitTest
{
    private IMediator _mediator;
    private IMapper _mapper;
    private SchoolContext _schoolContext;

    [SetUp]
    public async Task SetUp()
    {
        var builder = ShouldConfiguration.ShouldConfig();
        // 私有变量赋值
        var container = builder.Build();
        _schoolContext = container.Resolve<SchoolContext>();
        _mediator = container.Resolve<IMediator>();
        _mapper = container.Resolve<IMapper>();
    }

    [Test]
    public async Task ShouldGetStudentList()
    {
        var request = new GetStudentListRequest()
        {
            Name = "t"
        };
        //通过接口获取数据
        var res = await _mediator.RequestAsync<GetStudentListRequest, GetStudentListResponse>(request);
        var studentDtos = _schoolContext.Set<Student>().Where(x => x.Name.Contains(request.Name)).ToList()
            .Select(x => _mapper.Map<StudentDto>(x)).ToList();
        res.StudentDtoList.Count.ShouldBe(studentDtos.Count);
    }

    [Test]
    public async Task ShouldAddStudent()
    {
        var command = new AddStudentCommand()
        {
            Name = "snake",
            Age = 90
        };
        var response = await _mediator.SendAsync<AddStudentCommand, AddStudentResponse>(command);
        var student = _schoolContext.Set<Student>().FirstOrDefault(x => x.Id == response.StudentDto.Id);
        student.ShouldNotBeNull();
    }

    [Test]
    public async Task ShouldUpdateStudent()
    {
        string id = "2ceddce2-8064-4cbb-95d1-c1e382fe010c";
        var command = new UpdateStudentCommand()
        {
            Id = Guid.Parse(id),
            Name = "snake12",
            Age = 100
        };
        await _mediator.SendAsync(command);
        var student = _schoolContext.Set<Student>().FirstOrDefault(x => x.Id == Guid.Parse(id));
        var studentCommand = _mapper.Map<UpdateStudentCommand>(student);
        command.ShouldBeEquivalentTo(studentCommand);
    }
    
    [Test]
    public async Task ShouldDeleteStudent()
    {
        string id = "26470619-cf12-4569-bf08-442ca6ca69c1";
        var command = new DeleteStudentCommand()
        {
            Id = Guid.Parse(id),
            Name = "snake12",
            Age = 100
        };
        await _mediator.SendAsync(command);
        var student = _schoolContext.Set<Student>().FirstOrDefault(x => x.Id == Guid.Parse(id));
        student.ShouldBeNull();
    }
    
}