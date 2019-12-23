using System;
using Microsoft.AspNetCore.Http;

namespace FinalProj
{
    public class FileUpload
    {
        public IFormFile File { get; set; }
        public FileUpload()
        {
        }
    }
}
