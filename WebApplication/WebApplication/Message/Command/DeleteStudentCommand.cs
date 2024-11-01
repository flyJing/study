using ICommand = Mediator.Net.Contracts.ICommand;

namespace WebApplication.Message.Command;

public class DeleteStudentCommand: ICommand
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int Age { get; set; }
    
}