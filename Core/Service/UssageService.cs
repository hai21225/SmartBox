public class UsageService
{
    private float _price = 100f;

    private readonly UsageRepository _usageRepo;
    private  readonly LockerService _lockerService;
    private readonly CommandService _command;
    public UsageService(UsageRepository usageRepo, LockerService lockerService, CommandService command)
    {
        _usageRepo = usageRepo;
        _lockerService = lockerService;
        _command = command;
    }

    public async Task<string?> StartUsage(int userId, int lockerId)
    {
        var locker = await _lockerService.GetLockerById(lockerId);
        if (locker == null) return null;

        if (locker.Status == false) return null;
        var code = GenerateCode();

        var usage = new UsageDTO
        {
            UserId = userId,
            LockerId = lockerId,
            Status = "Using",
            StartTime = DateTime.Now,
            EndTime = null,
            TotalPrice = 0,
            AccessCode = code,
            CodeExpireAt = DateTime.Now.AddHours(12),
            IsUsed=false,
        };

        await _usageRepo.Add(usage);
        await _command.Open(lockerId);
        await _lockerService.SetStatus(lockerId,false);

        return code;
    }
    public async Task<bool> EndUsage(int lockerId)
    {
        
        var usage = await GetActiveUsage(lockerId);
        if (usage == null) return false;

        var now = DateTime.Now;
        if (usage.PriceExpireAt == null || now > usage.PriceExpireAt)
        {
            Console.WriteLine("check"+now.ToString());
            Console.WriteLine("cehckkkkk"+usage.PriceExpireAt.ToString());
            return false;
        }

        // update usage
        usage.EndTime = now;
        usage.TotalPrice = usage.TempPrice;
        usage.Status = "Done";

        await _usageRepo.Update(usage);

        await _command.Open(lockerId);

        // update locker
        var locker = await _lockerService.GetLockerById(lockerId);
        if (locker != null)
        {
            await _lockerService.SetStatus(locker.Id,true);
        }

        return true;
    }

    public async Task<bool> OpenByCode(string code)
    {
        var usage = await  _usageRepo.GetByCode(code);

        if (usage == null) return false;

        // check expire
        if (usage.CodeExpireAt != null && DateTime.Now > usage.CodeExpireAt)
            return false;

        await _command.Open(usage.LockerId);

        return true;
    }


    public async Task<bool> EndUsageByCode(string code)
    {
        var usage = await _usageRepo.GetByCode(code);

        if (usage == null) return false;

        var now = DateTime.Now;

        // 🔥 check payment expire (QUAN TRỌNG)
        if (usage.PriceExpireAt == null || now > usage.PriceExpireAt)
        {

            Console.WriteLine("Ditmemaythz lon ocs cac");
            return false;
        }

        // 🔥 dùng giá đã calculate
        usage.EndTime = now;
        usage.TotalPrice = usage.TempPrice;
        usage.Status = "Done";
        usage.IsUsed = true;

        await _usageRepo.Update(usage);

        await _command.Open(usage.LockerId);
        await _lockerService.SetStatus(usage.LockerId, true);

        return true;
    }
    // 3. Get usage by id
    public async Task<UsageDTO?> GetUsageById(int id)
    {
        return await _usageRepo.GetById(id);
    }

    public async Task<List<UsageDTO>> GetAllUsage()
    {
        return await _usageRepo.GetAll();
    }

    public async Task<UsageDTO?> GetActiveUsage(int lockerId)
    {
        var all = await _usageRepo.GetAll();

        return all.FirstOrDefault(x =>
            x.LockerId == lockerId &&
            x.Status == "Using");
    }

    public async Task<List<UsageDTO>> GetUsageHistoryByUser(int userId)
    {
        var all = await _usageRepo.GetAll();

        return all
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.StartTime)
            .ToList();
    }

    public float CalculatePrice(DateTime start, DateTime end)
    {
        var minutes = (end - start).TotalMinutes;

        if (minutes < 1) minutes = 1; // tối thiểu 1 phút

        return (float)(minutes * _price); //
    }


    public async Task<object?> Calculate(int lockerId)
    {
        var usage = await GetActiveUsage(lockerId);
        if (usage == null) return null;

        var now = DateTime.Now;
        var price = CalculatePrice(usage.StartTime, now);

        //lưu giá tạm + expire 10s
        usage.TempPrice = price;
        usage.PriceExpireAt = now.AddSeconds(10);

        await _usageRepo.Update(usage);

        return new
        {
            id = usage.Id,
            lockerId = lockerId,
            startTime = usage.StartTime,
            endTime = now,
            totalPrice = price,
            expireAt = usage.PriceExpireAt
        };
    }

    private string GenerateCode()
    {
        return new Random().Next(100000, 999999).ToString();
    }
    public async Task<object?> CalculateByCode(string code)
    {
        var usage = await _usageRepo.GetByCode(code);

        if (usage == null) return null;

        var now = DateTime.Now;

        // check expire code
        if (usage.CodeExpireAt != null && now > usage.CodeExpireAt)
            return null;

        var price = CalculatePrice(usage.StartTime, now);

        // lưu giá tạm + expire 10s (giống logic cũ)
        usage.TempPrice = price;
        usage.PriceExpireAt = now.AddSeconds(10);

        await _usageRepo.Update(usage);

        return new
        {
            id=usage.Id,
            lockerId = usage.LockerId,
            startTime = usage.StartTime,
            endTime = now,
            totalPrice = price,
            expireAt = usage.PriceExpireAt
        };
    }

}