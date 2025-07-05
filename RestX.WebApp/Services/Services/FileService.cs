using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Services
{
    public class FileService : BaseService, IFileService
    {
        private readonly IWebHostEnvironment environment;

        public FileService(IRepository repo, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment) 
            : base(repo, httpContextAccessor) 
        {
            environment = environment;
        }

        public async Task<Guid> CreateFileAsync(string name, string url, string userId)
        {
            var file = new Models.File
            {
                Id = Guid.NewGuid(),
                Name = name,
                Url = url
            };

            await Repo.CreateAsync(file, userId);
            await Repo.SaveAsync();
            return file.Id;
        }

        public async Task<Models.File?> GetFileByIdAsync(Guid fileId)
        {
            return await Repo.GetByIdAsync<Models.File>(fileId);
        }

        public async Task UpdateFileAsync(Guid fileId, string name, string url, string userId)
        {
            var file = await Repo.GetByIdAsync<Models.File>(fileId);
            if (file != null)
            {
                file.Name = name;
                file.Url = url;
                Repo.Update(file, userId);
                await Repo.SaveAsync();
            }
        }

        public async Task DeleteFileAsync(Guid fileId)
        {
            var file = await Repo.GetByIdAsync<Models.File>(fileId);
            if (file != null)
            {
                // Delete physical file if it exists
                if (!string.IsNullOrEmpty(file.Url) && file.Url.StartsWith("~/"))
                {
                    var physicalPath = Path.Combine(environment.WebRootPath, file.Url.Replace("~/", "").Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }

                Repo.Delete(file);
                await Repo.SaveAsync();
            }
        }

        public async Task<Models.File?> CreateOrUpdateFileAsync(Guid? existingFileId, string name, string url, string userId)
        {
            if (existingFileId.HasValue)
            {
                var existingFile = await GetFileByIdAsync(existingFileId.Value);
                if (existingFile != null)
                {
                    existingFile.Name = name;
                    existingFile.Url = url;
                    Repo.Update(existingFile, userId);
                    await Repo.SaveAsync();
                    return existingFile;
                }
            }

            var newFile = new Models.File
            {
                Id = Guid.NewGuid(),
                Name = name,
                Url = url
            };

            await Repo.CreateAsync(newFile, userId);
            await Repo.SaveAsync();
            return newFile;
        }

        public async Task<string> UploadDishImageAsync(IFormFile file, string ownerName, string dishName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type. Only image files are allowed.");

            var fileName = GetDishImagePath(ownerName, dishName, extension);
            var uploadsFolder = Path.Combine(environment.WebRootPath, "Uploads", "DishesImage");
            var filePath = Path.Combine(uploadsFolder, fileName);

            Directory.CreateDirectory(uploadsFolder);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"~/Uploads/DishesImage/{fileName}";
        }

        public async Task<bool> DeleteDishImageAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return false;

                var physicalPath = Path.Combine(environment.WebRootPath, filePath.Replace("~/", "").Replace("/", Path.DirectorySeparatorChar.ToString()));
                
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                    return true;
                }
                
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetDishImagePath(string ownerName, string dishName, string extension)
        {
            var sanitizedOwnerName = SanitizeFileName(ownerName);
            var sanitizedDishName = SanitizeFileName(dishName);
            
            return $"Dish-{sanitizedOwnerName}-{sanitizedDishName}{extension}";
        }

        public async Task<Models.File> CreateFileFromUploadAsync(string filePath, string fileName, string userId)
        {
            var file = new Models.File
            {
                Id = Guid.NewGuid(),
                Name = fileName,
                Url = filePath
            };

            await Repo.CreateAsync(file, userId);
            await Repo.SaveAsync();
            return file;
        }

        private string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "Unknown";

            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new string(fileName.Where(c => !invalidChars.Contains(c)).ToArray());
            return sanitized.Replace(" ", "_").Replace("-", "_");
        }
    }
}