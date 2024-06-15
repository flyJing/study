using FluentValidation;
using WebApplication1.Message.Command;

namespace WebApplication1.Validator;

public class UpdateStudentValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentValidator()
    {
        RuleFor(x => x.Name).NotNull().Length(2,50);
        RuleFor(x => x.Age).NotNull().ExclusiveBetween(0,200);
    }
    
    
}