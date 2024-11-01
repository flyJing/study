using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication.Message;
using WebApplication.Message.Command;
using WebApplication.Service;

namespace WebApplication.Handler.TeacherHandler;

public class UpdateTeacherCommandHandler: ICommandHandler<UpdateTeacherCommand>
{
    private readonly ITeacherService _teacherService;
    
    public UpdateTeacherCommandHandler(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    public async Task Handle(IReceiveContext<UpdateTeacherCommand> context, CancellationToken cancellationToken)
    {
        await _teacherService.UpdateTeacher(context.Message, cancellationToken);
    }
}