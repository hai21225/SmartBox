public class LockerService
{

    private readonly LockerRepository _locker;
    public LockerService(LockerRepository locker)
    {
        _locker = locker;
    }

    public async Task<List<LockerDTO>> GetAllLockers()
    {
        return await _locker.GetAll();
    }

    public async Task<LockerDTO?> GetLockerById(int id)
    {
        return await _locker.GetById(id);
    }
    public async Task<bool> IsAvailable(int id)
    {
        var locker= await _locker.GetById(id);
        if(locker?.Status==false) { return false; }

        return true;

    }
    public async Task<bool> SetStatus(int lockerId, bool status)
    {
        var item = new LockerDTO
        {
            Id = lockerId,
            Status = status
        };
        return await _locker.Update(item);
    }

}