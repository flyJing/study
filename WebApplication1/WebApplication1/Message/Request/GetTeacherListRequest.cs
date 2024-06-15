using Mediator.Net.Contracts;

namespace WebApplication1.Message.Request;

public class GetTeacherListRequest: IRequest
{
    public string? Name { get; set; }
}