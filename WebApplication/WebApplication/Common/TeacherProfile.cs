using AutoMapper;
using WebApplication.Dto;
using WebApplication.Entity;
using WebApplication.Message.Command;

namespace WebApplication.Common;

public class TeacherProfile: Profile
{
    public TeacherProfile()
    {
        CreateMap<Teacher, TeacherDto>();
        CreateMap<Teacher, UpdateTeacherCommand>();
        CreateMap<UpdateTeacherCommand, Teacher>();
        CreateMap<AddTeacherCommand, Teacher>();
        CreateMap<AddTeacherCommand, TeacherDto>();
        
    }
}