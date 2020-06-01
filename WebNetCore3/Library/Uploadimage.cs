using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetCore3.Library
{
    public class Uploadimage
    {
        public async Task<byte[]> ByteAvatarImageAsync(IFormFile AvatarImage, IWebHostEnvironment environment)
        {
            if (AvatarImage!= null)
            {
                // MemoryStream, alamcena en memoria la informacion de nuestra imagen
                using (var memoryStream = new MemoryStream())
                {
                    // Await, le inddica a nuestra metodo que el siguiente metodo (CopyToAsync) va a ejeutar una tarea
                    // Y que tiene que esperarlo hasta que termine su tarea. Una vez que termine su tarea va a proseguir con el codigo siguiente
                    await AvatarImage.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            else
            {
                var archivoOrigen = environment.ContentRootPath + "/wwwroot/Images/logo-google.png";
                return File.ReadAllBytes(archivoOrigen);
            }
        }
    }
}
