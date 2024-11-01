using Mediator.Net.Contracts;
using WebApplication.Dto;

namespace WebApplication.Message.Response;

public class AddTeacherResponse: IResponse
{
    public TeacherDto TeacherDto { get; set; }
}