using Mediator.Net.Context;
using Mediator.Net.Contracts;
using WebApplication1.Message.Command;
using WebApplication1.Service;
using WebApplication1.Validator;

namespace WebApplication1.Handler.StudentHandler;

/**
 * 删除处理 handler
 */
public class UpdateStudentCommandHandler : ICommandHandler<UpdateStudentCommand>
{
    private readonly IStudentService _studentService;
    private readonly UpdateStudentValidator _validator;

    public UpdateStudentCommandHandler(IStudentService studentService, UpdateStudentValidator updateStudentValidator)
    {
        _studentService = studentService;
        _validator = updateStudentValidator;
    }
    
    public async Task Handle(IReceiveContext<UpdateStudentCommand> context, CancellationToken cancellationToken)
    {
        if (_validator.Validate(context.Message).IsValid)
        {
            await _studentService.UpdateStudent(context.Message);
        }
        else
        {
            Console.WriteLine("参数验证失败");
        }
    }
}