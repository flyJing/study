using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication1.Message;
using WebApplication1.Message.Command;
using WebApplication1.Service;

namespace WebApplication1.Handler.TeacherHandler;

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