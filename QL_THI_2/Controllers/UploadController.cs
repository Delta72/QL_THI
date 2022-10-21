using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QL_THI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        public static string UploadImage(IFormFile img, string id)
        {
            var filename = Guid.NewGuid().ToString() + img.FileName;
            var filepath = Directory.GetCurrentDirectory() + "\\user\\" + id + "\\" + filename;
            using var image = SixLabors.ImageSharp.Image.Load(img.OpenReadStream());
            image.Mutate(x => x.Resize(200, 200));
            image.Save(filepath);
            var report = "/user/" + id + "/" + filename;
            return report;
        }

        public static string UploadFile(IFormFile file, string idN, string idTK, string currentDir)
        {
            var filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
            var filepath = currentDir + "\\wwwroot\\user\\" + idTK + "\\" + idN + "\\"+ filename;
            using (Stream fileStream = new FileStream(filepath, FileMode.Create))
            {
                file.CopyToAsync(fileStream);
            }
            string report = "/user/" + idTK + "/" + idN + "/"+ filename;
            return report;
        }

        public static void DeleteFile(string path, string currentDir)
        {
            var filepath = currentDir + "\\wwwroot" + path;
            if (System.IO.File.Exists(filepath)) { System.IO.File.Delete(Path.Combine(filepath)); }
        }
    }
}
