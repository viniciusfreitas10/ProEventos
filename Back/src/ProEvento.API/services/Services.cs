using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProEvento.API.services
{
    public class Services
    {
        public Services()
        {
        }
        public void DeleteImage(string imageName, IWebHostEnvironment _hostEnvironment)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);

            if (File.Exists(imagePath))
                File.Delete(imagePath);
        }

        public async Task<string> SaveImage(IFormFile imageFile, IWebHostEnvironment _hostEnvironment)
        {
            string ImageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10)
                .ToArray())
                .Replace(" ", "-") ;

            ImageName = $"{ImageName}{DateTime.UtcNow:yymmssfff}{Path.GetExtension(imageFile.FileName)}";

            var ImagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", ImageName);

            using(var fileStream = new FileStream(ImagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return ImageName;
        }
    }
}
