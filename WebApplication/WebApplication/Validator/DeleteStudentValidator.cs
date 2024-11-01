using FluentValidation;
using WebApplication.Message.Command;

namespace WebApplication.Validator;

public class DeleteStudentValidator : AbstractValidator<DeleteStudentCommand>
{
    public DeleteStudentValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Name).NotNull().Length(2,50);
        RuleFor(x => x.Age).NotNull().ExclusiveBetween(0,200);
    }
    
    
}