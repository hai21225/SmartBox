public class UsageDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int LockerId { get; set; }
    public string Status { get; set; } = "Done";
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public float TotalPrice { get; set; }

    public float TempPrice { get; set; }
    public DateTime? PriceExpireAt { get; set; }

    public string? AccessCode { get; set; }
    public DateTime? CodeExpireAt { get; set; }
    public bool IsUsed { get; set; } = false;
}