using FluentValidation;
using WebApplication1.Message.Command;

namespace WebApplication1.Validator;

public class DeleteStudentValidator : AbstractValidator<DeleteStudentCommand>
{
    public DeleteStudentValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Name).NotNull().Length(2,50);
        RuleFor(x => x.Age).NotNull().ExclusiveBetween(0,200);
    }
    
    
}