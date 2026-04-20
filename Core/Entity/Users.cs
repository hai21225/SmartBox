using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class Users
    {
        public int Id { get; set; }
        [Required] public string UserName { get; set; }= string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
