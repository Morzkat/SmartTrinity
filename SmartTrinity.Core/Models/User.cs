using SmartTrinity.Core.Database.Models;

namespace SmartTrinity.Core.Models
{
    public class User : BaseEntity
    {
        public new int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string RolId { get; set; }
        public string Code { get; set; }
    }
}