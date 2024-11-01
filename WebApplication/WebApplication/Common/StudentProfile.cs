using AutoMapper;
using WebApplication.Dto;
using WebApplication.Entity;
using WebApplication.Message.Command;


namespace WebApplication.Common;

public class StudentProfile: Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<StudentDto, Student>();
        
        CreateMap<DeleteStudentCommand, StudentDto>();
        CreateMap<UpdateStudentCommand, StudentDto>();
        CreateMap<Student, UpdateStudentCommand>();
    }
}