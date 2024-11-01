using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication.Message.Command;
using WebApplication.Service;
using WebApplication.Validator;

namespace WebApplication.Handler.StudentHandler;

/**
 * 删除处理 handler
 */
public class DeleteStudentCommandHandler : ICommandHandler<DeleteStudentCommand>
{
    private readonly IStudentService _studentService;
    private readonly DeleteStudentValidator _validator;

    public DeleteStudentCommandHandler(IStudentService studentService, DeleteStudentValidator deleteStudentValidator)
    {
        _studentService = studentService;
        _validator = deleteStudentValidator;
    }
    
    public async Task Handle(IReceiveContext<DeleteStudentCommand> context, CancellationToken cancellationToken)
    {
        if ((await _validator.ValidateAsync(context.Message, cancellationToken)).IsValid)
        {
            await _studentService.DeleteStudent(context.Message);
        }
        Console.WriteLine("参数验证失败");
    }
}