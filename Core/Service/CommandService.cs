public class CommandService
{

    private readonly CommandRepository _command;
    public  CommandService(CommandRepository command)
    {
        _command = command;
    }

    public async Task Open(int lockerId)
    {
        await _command.AddCommand(lockerId, "open");
    }

    public async  Task<CommandDTO?> GetPendingCommand(int lockerId)
    {
        var command= await _command.GetPendingCommand(lockerId);
        if (command == null) return null;

        return new CommandDTO
        {
            Id = command.Id,
            LockerId = command.LockerId,
            Action = command.Action,
            Status= command.Status,
        };
    }

    public async Task MarkDone(int id)
    {
        await _command.MarkDone(id);
    }
}