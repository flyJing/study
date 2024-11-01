using Mediator.Net.Contracts;

namespace WebApplication.Message.Command;

public class AddStudentCommand: ICommand
{
    public string? Name { get; set; }
    
    public int Age { get; set; }
    
}