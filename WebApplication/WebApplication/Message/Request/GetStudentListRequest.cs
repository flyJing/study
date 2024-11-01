using Mediator.Net.Contracts;

namespace WebApplication.Message.Request;

public class GetStudentListRequest: IRequest
{
    public string? Name { get; set; }
    
}