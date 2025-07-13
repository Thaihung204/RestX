using RestX.WebApp.Models.ViewModels;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IStaffService
    {
        public Task<StaffProfileViewModel> GetStaffByIdAsync(CancellationToken cancellationToken);
    }
}
