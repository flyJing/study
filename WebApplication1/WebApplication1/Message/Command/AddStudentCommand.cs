using Mediator.Net.Contracts;

namespace WebApplication1.Message.Command;

public class AddStudentCommand: ICommand
{
    public string? Name { get; set; }
    
    public int Age { get; set; }
    
}