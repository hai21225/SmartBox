public class CommandDTO
{
    public int Id { get; set; }
    public int LockerId { get; set; }
    public string Action { get; set; } = "close"; // open / close
    public string Status { get; set; } = "done"; // pending / done
    public DateTime CreatedAt { get; set; }
}