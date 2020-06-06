using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uploader.Models;
using Uploader.Services;

namespace Uploader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateAlbum([FromForm] Album album)
        {
            var response = await _albumService.CreateAlbum(album);
            return Ok(response);
        }
    }
}
