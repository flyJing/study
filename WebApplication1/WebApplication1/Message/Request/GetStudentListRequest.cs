using Mediator.Net.Contracts;

namespace WebApplication1.Message.Request;

public class GetStudentListRequest: IRequest
{
    public string? Name { get; set; }
    
}