using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace ContactBook.Infrastructure.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicUrl);
    }

    
}