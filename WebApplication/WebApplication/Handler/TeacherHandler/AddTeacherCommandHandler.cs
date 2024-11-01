using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication.Common;
using WebApplication.Message;
using WebApplication.Message.Command;
using WebApplication.Message.Response;
using WebApplication.Service;
using WebApplication.Validator;

namespace WebApplication.Handler.TeacherHandler;

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