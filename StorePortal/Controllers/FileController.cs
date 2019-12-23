using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProj.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<dynamic> UploadAsync(IFormCollection form)
        {
            try
            {
                FileUpload file = new FileUpload();
                file.File = (IFormFile)form.Files[0];
                //await Upload(file);
                //return new { Success = true };

                if(file.File.Length > 0)
                {
                    String path = ("uploadedFiles.csv");
                    using(var fs = new FileStream(path, FileMode.Create))
                    {
                        await file.File.CopyToAsync(fs);
                    }
                }
                return new { Success = true };
            }
            catch(Exception e)
            {
                return new { Success = false, e.Message };
            }
            
        }


        private async Task Upload(FileUpload files)
        {
            var file = files.File;

            if (file.Length > 0)
            {
                String path = ("uploadFiles");
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
            }
        }
    }
}
