using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Message.Command;

namespace WebApplication1.Validator;

public class AddStudentValidator : AbstractValidator<AddStudentCommand>
{
    public AddStudentValidator()
    {
        RuleFor(x => x.Name).NotNull().Length(2,50);
        RuleFor(x => x.Age).NotNull().ExclusiveBetween(0,200);
    }
    
    public ValidationResult Validate(EntityEntry entityEntry)
    {
        return Validate(entityEntry.Entity as AddStudentCommand);
    }
    
}