using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestX.WebApp.Models;
using RestX.WebApp.Models.Home;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class StaffService : BaseService, IStaffService
    {
        public StaffService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
        }

        public async Task<StaffProfileViewModel> GetStaffByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            
            var staffs = await Repo.GetAsync<Staff>(
                filter: s => s.Id == id,
                includeProperties: "File,Owner,Accounts"
            );

            var staff = staffs.FirstOrDefault();
            if (staff == null)
                return null;

            var staffViewModel = new StaffProfileViewModel
            {
                OwnerId = staff.OwnerId,
                FileId = staff.FileId ?? Guid.Empty, 
                Name = staff.Name,
                Email = staff.Email,
                Phone = staff.Phone,
                IsActive = staff.IsActive,
                File = staff.File,
                Owner = staff.Owner
            };

            return staffViewModel;
        }
    }
}