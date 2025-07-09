using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace RestX.WebApp.Services.Services
{
    public class StaffManagementService : BaseService, IStaffManagementService
    {
        private readonly IFileService fileService;
        private readonly IOwnerService ownerService;

        public StaffManagementService(IRepository repo, IHttpContextAccessor httpContextAccessor, IFileService fileService, IOwnerService ownerService)
            : base(repo, httpContextAccessor)
        {
            this.fileService = fileService;
            this.ownerService = ownerService;
        }

        public async Task<StaffManagementViewModel> GetStaffManagementViewModelAsync(Guid ownerId)
        {
            var staffs = await GetStaffsByOwnerIdAsync(ownerId);

            var staffViewModels = staffs.Select(staff => new StaffViewModel
            {
                Id = staff.Id,
                FileId = staff.FileId,
                ImageUrl = staff.File?.Url,
                Name = staff.Name,
                Email = staff.Email,
                Phone = staff.Phone,
                Username = staff.Accounts.FirstOrDefault()?.Username ?? string.Empty,
                Password = string.Empty,
                IsActive = staff.IsActive
            }).ToList();

            return new StaffManagementViewModel
            {
                Staffs = staffViewModels
            };
        }

        public async Task<List<Staff>> GetStaffsByOwnerIdAsync(Guid ownerId)
        {
            var staffs = await Repo.GetAsync<Staff>(
                filter: s => s.OwnerId == ownerId,
                includeProperties: "Accounts,File"
            );
            return staffs.OrderBy(s => s.Name).ToList();
        }

        public async Task<Staff?> GetStaffByIdAsync(Guid id)
        {
            var staffs = await Repo.GetAsync<Staff>(
                filter: s => s.Id == id,
                includeProperties: "Accounts,File"
            );
            return staffs.FirstOrDefault();
        }

        public async Task<StaffViewModel?> GetStaffViewModelByIdAsync(Guid staffId)
        {
            var staff = await GetStaffByIdAsync(staffId);
            if (staff == null) return null;

            return new StaffViewModel
            {
                Id = staff.Id,
                FileId = staff.FileId,
                ImageUrl = staff.File?.Url,
                Name = staff.Name,
                Email = staff.Email,
                Phone = staff.Phone,
                Username = staff.Accounts.FirstOrDefault()?.Username ?? string.Empty,
                Password = string.Empty,
                IsActive = staff.IsActive
            };
        }

        public async Task<Guid?> UpsertStaffAsync(StaffRequest request, Guid ownerId)
        {
            try
            {
                Staff staff;
                Account account;
                bool isEdit = request.Id.HasValue && request.Id.Value != Guid.Empty;

                if (isEdit)
                {
                    staff = await GetStaffByIdAsync(request.Id.Value);
                    if (staff == null)
                        return null;

                    account = staff.Accounts.FirstOrDefault();
                    if (account == null)
                        return null;

                    if (await IsUsernameExistsAsync(request.Username, account.Id))
                        throw new InvalidOperationException("Username is already taken.");

                    if (await IsEmailExistsAsync(request.Email, staff.Id))
                        throw new InvalidOperationException("Email is already taken.");

                    staff.Name = request.Name;
                    staff.Email = request.Email;
                    staff.Phone = request.Phone;
                    staff.IsActive = request.IsActive;

                    if (request.ImageFile != null && request.ImageFile.Length > 0)
                    {
                        var owner = await ownerService.GetOwnerByIdAsync(ownerId);
                        var imageUrl = await UploadStaffImageAsync(request.ImageFile, owner.Name, request.Name);
                        var file = await fileService.CreateFileFromUploadAsync(imageUrl, request.ImageFile.FileName, ownerId);
                        staff.FileId = file.Id;
                    }

                    account.Username = request.Username;
                    account.Password = HashPassword(request.Password);

                    Repo.Update(staff, ownerId.ToString());
                    Repo.Update(account, ownerId.ToString());
                    await Repo.SaveAsync();
                    return staff.Id;
                }
                else
                {
                    if (await IsUsernameExistsAsync(request.Username))
                        throw new InvalidOperationException("Username is already taken.");

                    if (await IsEmailExistsAsync(request.Email))
                        throw new InvalidOperationException("Email is already taken.");

                    staff = new Staff
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = ownerId,
                        Name = request.Name,
                        Email = request.Email,
                        Phone = request.Phone,
                        IsActive = request.IsActive,
                        FileId = null,
                        CreatedDate = DateTime.UtcNow
                    };

                    if (request.ImageFile != null && request.ImageFile.Length > 0)
                    {
                        var owner = await ownerService.GetOwnerByIdAsync(ownerId);
                        var imageUrl = await UploadStaffImageAsync(request.ImageFile, owner.Name, request.Name);
                        var file = await fileService.CreateFileFromUploadAsync(imageUrl, request.ImageFile.FileName, ownerId);
                        staff.FileId = file.Id;
                    }

                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        StaffId = staff.Id,
                        Username = request.Username,
                        Password = HashPassword(request.Password),
                        Role = "Staff",
                        CreatedDate = DateTime.UtcNow
                    };

                    await Repo.CreateAsync(staff, ownerId.ToString());
                    await Repo.CreateAsync(account, ownerId.ToString());
                    await Repo.SaveAsync();
                    return staff.Id;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error saving staff: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteStaffAsync(Guid staffId)
        {
            try
            {
                var staff = await GetStaffByIdAsync(staffId);
                if (staff == null)
                    return false;

                if (staff.Accounts != null && staff.Accounts.Any())
                {
                    var accountsList = staff.Accounts.ToList();
                    foreach (var account in accountsList)
                    {
                        Repo.Delete<Account>(account);
                    }

                    await Repo.SaveAsync();
                }

                if (staff.FileId.HasValue)
                {
                    try
                    {
                        await fileService.DeleteFileAsync(staff.FileId.Value);
                    }
                    catch (Exception fileEx)
                    {
                        Console.WriteLine($"Warning: Could not delete file: {fileEx.Message}");
                    }
                }

                Repo.Delete<Staff>(staff);
                await Repo.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete staff: {ex.Message}", ex);
            }
        }

        private async Task<string> UploadStaffImageAsync(IFormFile file, string ownerName, string staffName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type. Only image files are allowed.");

            var fileName = GetStaffImagePath(ownerName, staffName, extension);
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "StaffImages");
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

            return $"~/Uploads/StaffImages/{fileName}";
        }

        private string GetStaffImagePath(string ownerName, string staffName, string extension)
        {
            var sanitizedOwnerName = SanitizeFileName(ownerName);
            var sanitizedStaffName = SanitizeFileName(staffName);

            return $"Staff-{sanitizedOwnerName}-{sanitizedStaffName}{extension}";
        }

        private string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "Unknown";

            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new string(fileName.Where(c => !invalidChars.Contains(c)).ToArray());
            return sanitized.Replace(" ", "_").Replace("-", "_");
        }

        private async Task<bool> IsUsernameExistsAsync(string username, Guid? excludeAccountId = null)
        {
            var accounts = await Repo.GetAsync<Account>(
                filter: a => a.Username == username && (excludeAccountId == null || a.Id != excludeAccountId.Value)
            );
            return accounts.Any();
        }

        private async Task<bool> IsEmailExistsAsync(string email, Guid? excludeStaffId = null)
        {
            var staffs = await Repo.GetAsync<Staff>(
                filter: s => s.Email == email && (excludeStaffId == null || s.Id != excludeStaffId.Value)
            );
            return staffs.Any();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}