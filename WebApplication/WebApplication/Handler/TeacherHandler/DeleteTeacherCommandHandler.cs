using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication.Message;
using WebApplication.Message.Command;
using WebApplication.Service;

namespace WebApplication.Handler.TeacherHandler;

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