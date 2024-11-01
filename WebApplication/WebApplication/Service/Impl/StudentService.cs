using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication.ContextConfiguration;
using WebApplication.Dto;
using WebApplication.Message.Command;
using WebApplication.Message.Request;

namespace WebApplication.Service.Impl;

public class StudentService : IStudentService
{
    private readonly SchoolContext _schoolContext;

    private readonly IMapper _mapper;
    
    public StudentService(SchoolContext schoolContext, IMapper mapper)
    {
        _schoolContext = schoolContext;
        _mapper = mapper;
    }

    public async Task<StudentDto> AddStudent(AddStudentCommand command)
    {
        var student = new Entity.Student()
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Age = command.Age
        };
        await _schoolContext.AddAsync(student);
        await _schoolContext.SaveChangesAsync();
        var studentDto = _mapper.Map<StudentDto>(student);
        return studentDto;
    }

    public async Task DeleteStudent(DeleteStudentCommand command)
    {
        var dbStudent = await _schoolContext.Set<Entity.Student>().SingleAsync(x => x.Id == command.Id);
        _schoolContext.Remove(dbStudent);
        await _schoolContext.SaveChangesAsync();
    }

    public async Task UpdateStudent(UpdateStudentCommand command)
    {
        var dbStudent = await _schoolContext.Set<Entity.Student>().FirstOrDefaultAsync(x => x.Id == command.Id);
        if (dbStudent == null) return;
        dbStudent.Name = command.Name;
        dbStudent.Age = command.Age;
        await _schoolContext.SaveChangesAsync();
    }

    public async Task<List<StudentDto>> StudentList(GetStudentListRequest request)
    {
        if (request.Name == null)
        {
            
            return await _schoolContext.Set<Entity.Student>().Select(x => _mapper.Map<StudentDto>(x)).ToListAsync();
        } 
        var studentDtoList = await _schoolContext.Set<Entity.Student>().Where(x => x.Name.Contains(request.Name))
            .Select(x => _mapper.Map<StudentDto>(x)).ToListAsync();
        return studentDtoList;
    }
}