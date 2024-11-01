using Mediator.Net.Contracts;

namespace WebApplication.Message.Command;

public class UpdateStudentCommand: ICommand
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int Age { get; set; }
    
}