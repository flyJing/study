using Mediator.Net.Contracts;

namespace WebApplication.Message.Request;

public class GetTeacherListRequest: IRequest
{
    public string? Name { get; set; }
}