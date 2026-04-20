using Core.Entity;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IBase<UserDTO>
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserDTO?> GetUserByUsername(string username)
    {
        var exits =await _context.Users.FirstOrDefaultAsync(x=>x.UserName==username);
        if(exits==null) {return null; }

        var user = new UserDTO
        {
            Id = exits.Id,
            UserName = username,
            Password = exits.Password
        };
        return user;
    }

    public async Task<bool> Add(UserDTO item)
    {
        var exists = await _context.Users
                    .AnyAsync(x => x.UserName == item.UserName);

        if (exists) return false;

        var user = new Users
        {
            UserName = item.UserName,
            Password = item.Password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<UserDTO>> GetAll()
    {
        return await _context.Users
        .Select(u => new UserDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                Password = u.Password
            }).ToListAsync();
    }

    public async Task<UserDTO?> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = user.Password
        };
    }

    public async Task<bool> Update(UserDTO item)
    {
        var user = await _context.Users.FindAsync(item.Id);
        if (user == null) return false;

        user.UserName = item.UserName;
        user.Password = item.Password;

        await _context.SaveChangesAsync();

        return true;
    }
}