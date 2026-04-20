using Microsoft.EntityFrameworkCore;

public class CommandRepository
{
    private readonly AppDbContext _context;

    public CommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddCommand(int lockerId, string action)
    {
        var cmd = new Command
        {
            LockerId = lockerId,
            Action = action,
            Status = "pending",
            CreatedAt = DateTime.Now
        };

        _context.Commands.Add(cmd);
        await _context.SaveChangesAsync();
    }

    public async Task<Command?> GetPendingCommand(int lockerId)
    {
        return await _context.Commands
            .Where(x => x.LockerId == lockerId && x.Status == "pending")
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task MarkDone(int id)
    {
        var cmd = await _context.Commands.FindAsync(id);
        if (cmd != null)
        {
            cmd.Status = "done";
            await _context.SaveChangesAsync();
        }
    }

}