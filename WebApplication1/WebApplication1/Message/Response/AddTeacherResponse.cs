using Mediator.Net.Contracts;
using WebApplication1.Dto;

namespace WebApplication1.Message.Response;

public class AddTeacherResponse: IResponse
{
    public TeacherDto TeacherDto { get; set; }
}