using AutoMapper;
using WebApplication1.Dto;
using WebApplication1.Entity;
using WebApplication1.Message;
using WebApplication1.Message.Command;

namespace WebApplication1.Common;

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