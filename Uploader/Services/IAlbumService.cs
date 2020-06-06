using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Uploader.Models;

namespace Uploader.Services
{
    public interface IAlbumService
    {
        public Task<IdResponse> CreateAlbum(Album album);
        public Task<Response> UploadImage(string fileName, string filePath, string containerId);
    }
}
