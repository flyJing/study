using Mediator.Net.Contracts;

namespace WebApplication.Message.Command;

public class UpdateTeacherCommand: ICommand
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int Age { get; set; }
}