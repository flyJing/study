using Autofac;
using Mediator.Net;
using Shouldly;
using WebApplication.ContextConfiguration;
using WebApplication.Dto;
using WebApplication.Entity;
using WebApplication.Message.Command;
using WebApplication.Message.Request;
using WebApplication.Message.Response;
using Xunit;
using IMapper = AutoMapper.IMapper;

namespace Test;

public class StudentUnitTest: IDisposable
{
    private IMediator _mediator;
    private IMapper _mapper;
    private SchoolContext _schoolContext;
    
    public StudentUnitTest()
    {
        var builder = ShouldConfiguration.ShouldConfig();
        // 私有变量赋值
        var container = builder.Build();
        _schoolContext = container.Resolve<SchoolContext>();
        _mediator = container.Resolve<IMediator>();
        _mapper = container.Resolve<IMapper>();
    }

    [Fact]
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

    [Fact]
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

    [Fact]
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
    
    [Fact]
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

    public void Dispose()
    {
        _mediator.Dispose();
        _schoolContext.Dispose();
    }
}