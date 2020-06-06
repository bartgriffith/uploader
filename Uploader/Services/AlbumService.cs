using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.IO;
using System.Threading.Tasks;
using Uploader.Models;

namespace Uploader.Services
{
    public class AlbumService : IAlbumService
    {
        const string storageconn = "DefaultEndpointsProtocol=https;AccountName=coastalphotostore;AccountKey=iPNAxTeNSwqjYTSnAldHJ7QriXwNycPWE+EMNJquC2I+zdx6nPsNnsxdmyBPjWc/b9DHhL940yIktsfBJxZCJw==;EndpointSuffix=core.windows.net";
        const string table = "Album";

        private readonly ICustomerService _customerService;
        private CloudTable _albumTable;

        public AlbumService(ICustomerService customerService)
        {
            _customerService = customerService;

            var storageacct = CloudStorageAccount.Parse(storageconn);
            var tblclient = storageacct.CreateCloudTableClient(new TableClientConfiguration());
            _albumTable = tblclient.GetTableReference(table);
        }

        public async Task<IdResponse> CreateAlbum(Album album)
        {
            try
            {
                // Todo: need to lookup actual admin id here
                var albumEntity = new AlbumEntity(album.Name, _customerService.GetCustomerIdByName(album.CustomerName));
                albumEntity.AccessKey = "generate access key";

                var insertAlbumOperation = TableOperation.InsertOrMerge(albumEntity);
                var result = await _albumTable.ExecuteAsync(insertAlbumOperation);

                var blobServiceClient = new BlobServiceClient(storageconn);
                var containerClient = await blobServiceClient.CreateBlobContainerAsync(albumEntity.AlbumId);

                return new IdResponse
                {
                    Id = albumEntity.AlbumId
                };
            }
            catch (Exception e)
            {
                return new IdResponse
                {
                    ErrorsOccurred = true
                };
            }            
        }

        public async Task<Response> UploadImage(string fileName, string filePath, string containerId)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(storageconn);
                var containerCliet = blobServiceClient.GetBlobContainerClient(containerId);

                var blobClient = containerCliet.GetBlobClient(fileName);

                using FileStream uploadFileStream = File.OpenRead(filePath);

                await blobClient.UploadAsync(uploadFileStream, true);
                uploadFileStream.Close();

                File.Delete(filePath);

                return new Response();
            }
            catch (Exception e)
            {
                return new Response
                {
                    ErrorsOccurred = true
                };
            }
        }
    }
}
