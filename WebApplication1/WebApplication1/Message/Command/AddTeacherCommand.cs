using Mediator.Net.Contracts;

namespace WebApplication1.Message.Command;

public class AddTeacherCommand: ICommand
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    
    public int Age { get; set; }
}