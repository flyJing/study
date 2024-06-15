using Mediator.Net.Contracts;

namespace WebApplication1.Message.Command;

public class DeleteTeacherCommand: ICommand
{
    public Guid Id { get; set; }
}