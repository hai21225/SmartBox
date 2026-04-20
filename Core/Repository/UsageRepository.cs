using Core.Entity;
using Microsoft.EntityFrameworkCore;

public class UsageRepository : IBase<UsageDTO>
{
    private readonly AppDbContext _db;
    public UsageRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<bool> Add(UsageDTO item)
    {
        var usage = new Usage
        {
            UserId = item.UserId,
            LockerId = item.LockerId,
            Status = item.Status,
            StartTime = item.StartTime,
            EndTime = item.EndTime,
            TotalPrice = item.TotalPrice,
            TempPrice = 0,
            PriceExpireAt = null,
            AccessCode = item.AccessCode,
            CodeExpireAt = item.CodeExpireAt,
            IsUsed = item.IsUsed
        };

        _db.Usages.Add(usage);
        await _db.SaveChangesAsync();

        return true;
    }
    public async Task<bool> Delete(int id)
    {
        var usage = await _db.Usages.FindAsync(id);
        if (usage == null) return false;

        _db.Usages.Remove(usage);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<List<UsageDTO>> GetAll()
    {
        return await _db.Usages
            .Select(u => new UsageDTO
            {
                Id = u.Id,
                UserId = u.UserId,
                LockerId = u.LockerId,
                Status = u.Status,
                StartTime = u.StartTime,
                EndTime = u.EndTime,
                TotalPrice = u.TotalPrice,
                PriceExpireAt=u.PriceExpireAt,
                TempPrice=u.TempPrice,
                AccessCode = u.AccessCode,
                CodeExpireAt = u.CodeExpireAt,
                IsUsed = u.IsUsed
            })
            .ToListAsync();
    }

    public async Task<UsageDTO?> GetById(int id)
    {
        var u = await _db.Usages.FindAsync(id);
        if (u == null) return null;

        return new UsageDTO
        {
            Id = u.Id,
            UserId = u.UserId,
            LockerId = u.LockerId,
            Status = u.Status,
            StartTime = u.StartTime,
            EndTime = u.EndTime,
            TotalPrice = u.TotalPrice,
            TempPrice = u.TempPrice,
            PriceExpireAt = u.PriceExpireAt,
            AccessCode = u.AccessCode,
            CodeExpireAt = u.CodeExpireAt,
            IsUsed = u.IsUsed
        };
    }

    public async Task<bool> Update(UsageDTO item)
    {
        var u = await _db.Usages.FindAsync(item.Id);
        if (u == null) return false;

        u.Status = item.Status;
        //u.StartTime = item.StartTime;
        u.EndTime = item.EndTime;
        u.TotalPrice = item.TotalPrice;
        u.TempPrice= item.TempPrice;
        u.PriceExpireAt=item.PriceExpireAt;
        u.AccessCode = item.AccessCode;
        //u.CodeExpireAt = item.CodeExpireAt;
        u.IsUsed = item.IsUsed;
        Console.WriteLine("Chekc isused:" + item.IsUsed.ToString());
        await _db.SaveChangesAsync();

        Console.WriteLine("Chekkkkkkkadadasd");
        
        return true;
    }


    public async Task<UsageDTO?> GetByCode(string code)
    {
        var u = await _db.Usages
            .FirstOrDefaultAsync(x => x.AccessCode == code && x.Status == "Using");

        if (u == null) return null;

        return new UsageDTO
        {
            Id = u.Id,
            UserId = u.UserId,
            LockerId = u.LockerId,
            Status = u.Status,
            StartTime = u.StartTime,
            EndTime = u.EndTime,
            TotalPrice = u.TotalPrice,
            TempPrice = u.TempPrice,
            PriceExpireAt = u.PriceExpireAt,
            AccessCode = u.AccessCode,
            CodeExpireAt = u.CodeExpireAt,
            IsUsed = u.IsUsed
        };
    }
}