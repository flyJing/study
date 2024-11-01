using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication.Message;
using WebApplication.Message.Request;
using WebApplication.Message.Response;
using WebApplication.Service;

namespace WebApplication.Handler.TeacherHandler;

public class GetTeacherListRequestHandler: IRequestHandler<GetTeacherListRequest,GetTeacherListResponse>
{

    private readonly ITeacherService _teacherService;
    
    public GetTeacherListRequestHandler(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    public async Task<GetTeacherListResponse> Handle(IReceiveContext<GetTeacherListRequest> context, CancellationToken cancellationToken)
    {
        var teacherDtoList = await _teacherService.TeacherList(context.Message, cancellationToken);
        var response = new GetTeacherListResponse()
        {
            TeacherDtoList = teacherDtoList
        };
        return response;
    }
}