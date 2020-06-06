using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uploader.Models;
using Uploader.Services;

namespace Uploader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public UploadController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public string Check()
        {
            return "Herro?";
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(
            [FromForm] UploadRequest request,
            [FromServices] IWebHostEnvironment environment)
        {
            var file = Request.Form.Files[0];            

            string filePath = $"{environment.WebRootPath}\\UploadedImages\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            var response = await _albumService.UploadImage(file.FileName, filePath, request.ContainerId);

            return Ok(response);
        }
    }
}
