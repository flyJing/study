using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication1.Message.Command;
using WebApplication1.Message.Response;
using WebApplication1.Service;
using WebApplication1.Validator;

namespace WebApplication1.Handler.StudentHandler;

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