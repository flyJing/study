using Autofac;
using AutoMapper;
using Mediator.Net;
using Mediator.Net.Autofac;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Respawn;
using Respawn.Graph;
using Shouldly;
using WebApplication1.Common;
using WebApplication1.ContextConfiguration;
using WebApplication1.Entity;
using WebApplication1.Handler;
using WebApplication1.Message;
using WebApplication1.Message.Command;
using WebApplication1.Message.Request;
using WebApplication1.Message.Response;

namespace TestProject1;

public class TeacherUnitTest
{
    private IMediator _mediator;
    private IMapper _mapper;
    private SchoolContext _schoolContext;
    private Respawner _respawner;

    [SetUp]
    public async Task SetUp()
    {
        var builder =  ShouldConfiguration.ShouldConfig();
        // 私有变量赋值
        var container = builder.Build();
        _schoolContext = container.Resolve<SchoolContext>();
        _mediator = container.Resolve<IMediator>();
        _mapper = container.Resolve<IMapper>();
        _respawner = await ShouldConfiguration.ShouldRespawnerConfig(new Table[]{"teacher"});
        await _respawner.ResetAsync(ShouldConfiguration.Connection);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _respawner.ResetAsync(ShouldConfiguration.Connection);
        await ShouldConfiguration.Connection.CloseAsync();
    }
    
    [Test]
    public async Task ShouldGetTeacherList()
    {
        var id = Guid.NewGuid();
        var command = new AddTeacherCommand()
        {
            Id = id,
            Name = "thejing1",
            Age = 18
        };
        await _mediator.SendAsync<AddTeacherCommand, AddTeacherResponse>(command);
        var response = await _mediator.RequestAsync<GetTeacherListRequest, GetTeacherListResponse>(new GetTeacherListRequest()
        {
            Name = command.Name
        });
        response.TeacherDtoList.Count.ShouldBe(1);
        
        // 没有name值时
        var noParamsResponse = await _mediator.RequestAsync<GetTeacherListRequest, GetTeacherListResponse>(new GetTeacherListRequest());
        noParamsResponse.TeacherDtoList.Count.ShouldBe(1);
        
    }
    
    [Test]
    public async Task ShouldAddTeacher()
    {
        var list = new List<string>()
        {
            "小米",
            "华为",
            "oppo",
            "vivo",
            "一加",
            "荣耀",
            "三星",
            "苹果",
            "三星"
        };
        var random = new Random();
        for (int i = 0; i < 10000000; i++)
        {
            var id = new Guid();
            var command = new AddTeacherCommand()
            {
                Id = id,
                Name = list[random.Next(0, list.Count)],
                Age = random.Next(0, 100)
            };
            var response = await _mediator.SendAsync<AddTeacherCommand, AddTeacherResponse>(command);
        }
    }
    
    [Test]
    public async Task ShouldUpdateTeacher()
    {
        var id = Guid.NewGuid();
        var addTeacherCommand = new AddTeacherCommand()
        {
            Id = id,
            Name = "missfist",
            Age = 12
        };
        await _mediator.SendAsync<AddTeacherCommand, AddTeacherResponse>(addTeacherCommand);
        
        var command = new UpdateTeacherCommand()
        { 
            Id= id,
            Name = "misstwo",
            Age = 18
        };
        await _mediator.SendAsync(command);
        var teacher = await _schoolContext.Set<Teacher>().SingleOrDefaultAsync(t => t.Id == command.Id);
        command.Name.ShouldBe(teacher.Name);
        command.Age.ShouldBe(teacher.Age);
    }
    
    [Test]
    public async Task ShouldDeleteTeacher()
    {
        var id = Guid.NewGuid();
        var addTeacherCommand = new AddTeacherCommand()
        {
            Id = id,
            Name = "test3",
            Age = 12
        };
        await _mediator.SendAsync<AddTeacherCommand, AddTeacherResponse>(addTeacherCommand);
        
        var command = new DeleteTeacherCommand()
        { 
            Id= id
        };
        await _mediator.SendAsync(command);
        var teacher = await _schoolContext.Set<Teacher>().SingleOrDefaultAsync(t => t.Id == command.Id);
        teacher.ShouldBeNull();
    }
}