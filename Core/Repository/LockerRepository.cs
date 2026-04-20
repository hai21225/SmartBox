
using Microsoft.EntityFrameworkCore;

public class LockerRepository : IBase<LockerDTO>
{
    private readonly AppDbContext _db;
    public LockerRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<bool> Add(LockerDTO item)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<LockerDTO>> GetAll()
    {
        return await _db.Lockers
            .Select(l => new LockerDTO
            {
                Id = l.Id,
                Status = l.Status
            })
            .ToListAsync();
    }

    public async Task<LockerDTO?> GetById(int id)
    {
        var locker = await _db.Lockers.FindAsync(id);

        if (locker == null) return null;

        return new LockerDTO
        {
            Id = locker.Id,
            Status = locker.Status
        };
    }

    public async Task<bool> Update(LockerDTO item)
    {
        var locker = await _db.Lockers.FindAsync(item.Id);
        if (locker == null) return false;

        locker.Status = item.Status;

        await _db.SaveChangesAsync();
        return true;
    }
}