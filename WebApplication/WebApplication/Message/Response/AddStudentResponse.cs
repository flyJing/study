using Mediator.Net.Contracts;
using WebApplication.Dto;

namespace WebApplication.Message.Response;

public class AddStudentResponse: IResponse
{
    public StudentDto StudentDto { get; set; }
}