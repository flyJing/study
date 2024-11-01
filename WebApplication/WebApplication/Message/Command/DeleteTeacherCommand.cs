using Mediator.Net.Contracts;

namespace WebApplication.Message.Command;

public class DeleteTeacherCommand: ICommand
{
    public Guid Id { get; set; }
}