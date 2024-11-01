using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication.Message.Command;
using WebApplication.Message.Response;
using WebApplication.Service;
using WebApplication.Validator;

namespace WebApplication.Handler.StudentHandler;

public class AddStudentCommandHandler : ICommandHandler<AddStudentCommand,AddStudentResponse>
{
    private readonly IStudentService _studentService;
    private readonly AddStudentValidator _validator;

    public AddStudentCommandHandler(IStudentService studentService, AddStudentValidator addStudentValidator)
    {
        _studentService = studentService;
        _validator = addStudentValidator;
    }

    public async Task<AddStudentResponse> Handle(IReceiveContext<AddStudentCommand> context, CancellationToken cancellationToken)
    {
        var response = new AddStudentResponse();
        if (_validator.Validate(context.Message).IsValid)
        {
            return response;
        }
        var result = await _studentService.AddStudent(context.Message);
        response.StudentDto = result;
        return response;
    }
}