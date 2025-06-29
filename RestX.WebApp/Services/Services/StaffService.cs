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
        public StaffService(IRepository repo) : base(repo)
        {
        }

        public async Task<StaffProfileViewModel> GetStaffByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var staff = await repo.GetByIdAsync<Staff>(id);

            var staffViewModel = new StaffProfileViewModel
            {
                OwnerId = staff.OwnerId,
                FileId = staff.FileId,
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
