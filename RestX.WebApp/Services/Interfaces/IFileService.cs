using RestX.WebApp.Models;
using System;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IFileService
    {
        Task<Guid> CreateFileAsync(string name, string url, string userId);
        Task<Models.File?> GetFileByIdAsync(Guid fileId);
        Task UpdateFileAsync(Guid fileId, string name, string url, string userId);
        Task DeleteFileAsync(Guid fileId);
        Task<Models.File?> CreateOrUpdateFileAsync(Guid? existingFileId, string name, string url, string userId);
        
        // New methods for file upload
        Task<string> UploadDishImageAsync(IFormFile file, string ownerName, string dishName);
        Task<bool> DeleteDishImageAsync(string filePath);
        string GetDishImagePath(string ownerName, string dishName, string extension);
        Task<Models.File> CreateFileFromUploadAsync(string filePath, string fileName, string userId);
    }
}