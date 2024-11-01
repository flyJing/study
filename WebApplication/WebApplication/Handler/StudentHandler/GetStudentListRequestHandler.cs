using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication.Message.Request;
using WebApplication.Message.Response;
using WebApplication.Service;

namespace WebApplication.Handler.StudentHandler;

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