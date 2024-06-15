using Mediator.Net.Contracts;
using WebApplication1.Dto;

namespace WebApplication1.Message.Response;

public class AddStudentResponse: IResponse
{
    public StudentDto StudentDto { get; set; }
}