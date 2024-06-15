using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication1.Message;
using WebApplication1.Message.Command;
using WebApplication1.Service;

namespace WebApplication1.Handler.TeacherHandler;

public class DeleteTeacherCommandHandler: ICommandHandler<DeleteTeacherCommand>
{
    private readonly ITeacherService _teacherService;
    
    
    public DeleteTeacherCommandHandler(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    public async Task Handle(IReceiveContext<DeleteTeacherCommand> context, CancellationToken cancellationToken)
    {
        await _teacherService.DeleteTeacher(context.Message, cancellationToken);
    }
}