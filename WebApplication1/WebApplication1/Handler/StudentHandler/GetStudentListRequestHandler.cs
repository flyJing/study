using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication1.Message.Request;
using WebApplication1.Message.Response;
using WebApplication1.Service;

namespace WebApplication1.Handler.StudentHandler;

public class GetStudentListRequestHandler : IRequestHandler<GetStudentListRequest, GetStudentListResponse>
{
    private readonly IStudentService _studentService;

    public GetStudentListRequestHandler(IStudentService studentService)
    {
        _studentService = studentService;
    }
    
    public async Task<GetStudentListResponse> Handle(IReceiveContext<GetStudentListRequest> context, CancellationToken cancellationToken)
    {
        var studentDtoList = await _studentService.StudentList(context.Message);
        var response = new GetStudentListResponse
        {
            StudentDtoList = studentDtoList
        };
        return response;
    }
}