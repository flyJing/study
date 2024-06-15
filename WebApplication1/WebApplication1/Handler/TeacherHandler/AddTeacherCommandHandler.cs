using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication1.Common;
using WebApplication1.Message;
using WebApplication1.Message.Command;
using WebApplication1.Message.Response;
using WebApplication1.Service;
using WebApplication1.Validator;

namespace WebApplication1.Handler.TeacherHandler;

public class AddTeacherCommandHandler: ICommandHandler<AddTeacherCommand,AddTeacherResponse>
{
    private readonly ITeacherService _teacherService;
    private readonly AddTeacherValidator _validator;
    
    public AddTeacherCommandHandler(ITeacherService teacherService, AddTeacherValidator addTeacherValidator)
    {
        _teacherService = teacherService;
        _validator = addTeacherValidator;
    }


    public async Task<AddTeacherResponse> Handle(IReceiveContext<AddTeacherCommand> context, CancellationToken cancellationToken)
    {
        // if (!(await _validator.ValidateAsync(context.Message, cancellationToken)).IsValid) throw new Exception();
        var teacherDto = await _teacherService.AddTeacher(context.Message,cancellationToken);
        var response = new AddTeacherResponse()
        {
            TeacherDto = teacherDto
        };
        return response;
    }
}