using FluentValidation;
using WebApplication.Message.Command;

namespace WebApplication.Validator;

public class UpdateStudentValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentValidator()
    {
        RuleFor(x => x.Name).NotNull().Length(2,50);
        RuleFor(x => x.Age).NotNull().ExclusiveBetween(0,200);
    }
    
    
}