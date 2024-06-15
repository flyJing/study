using AutoMapper;
using WebApplication1.Dto;
using WebApplication1.Message;
using WebApplication1.Message.Command;

namespace WebApplication1.Common;

public class StudentProfile: Profile
{
    public StudentProfile()
    {
        CreateMap<Entity.Student, StudentDto>();
        CreateMap<StudentDto, Entity.Student>();
        
        CreateMap<DeleteStudentCommand, StudentDto>();
        CreateMap<UpdateStudentCommand, StudentDto>();
        CreateMap<Entity.Student, UpdateStudentCommand>();
    }
}