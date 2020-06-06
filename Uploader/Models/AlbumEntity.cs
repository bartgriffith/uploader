using Microsoft.Azure.Cosmos.Table;
using System;

namespace Uploader.Models
{
    public class AlbumEntity : TableEntity
    {
        public AlbumEntity(string albumName, string customerId)
        {
            PartitionKey = customerId;
            RowKey = albumName;
        }
        public string AccessKey { get; set; }
        public string AlbumId { get; set; } = Guid.NewGuid().ToString();
    }
}
