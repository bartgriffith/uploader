using Microsoft.Azure.Cosmos.Table;

namespace Uploader.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
