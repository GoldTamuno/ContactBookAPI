using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBook.Infrastructure.Helper;
using ContactBook.Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ContactBook.Infrastructure.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult(); //method for uploading images
            if (file.Length > 0) //Checks if there is at least 1 file
            {
                using var stream = file.OpenReadStream(); //Reads the file

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream), //Grabs the name of the file that is uploaded
                   // Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face") //transforms the image
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);  //uploads the file to cloudinary
            }

            return uploadResult;
        }


        public Task<DeletionResult> DeletePhotoAsync(string publicUrl)
        {
            throw new NotImplementedException();
        }
    }
}