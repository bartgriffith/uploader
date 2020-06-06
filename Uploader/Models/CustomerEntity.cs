using Microsoft.Azure.Cosmos.Table;
using System;

namespace Uploader.Models
{
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity()
        {

        }
        public CustomerEntity(string name, string adminId)
        {
            PartitionKey = adminId;
            RowKey = name;
        }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CustomerId { get; set; } = Guid.NewGuid().ToString();
    }
}
