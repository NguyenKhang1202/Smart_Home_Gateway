using Gateway.Core.Entities.Base;

namespace Gateway.Core.Entities
{
    public class User : MongoEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

    }
}
