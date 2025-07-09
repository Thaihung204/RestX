using RestX.WebApp.Models.ViewModels;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IStaffManagementService
    {
        Task<StaffManagementViewModel> GetStaffManagementViewModelAsync(Guid ownerId);
        Task<StaffViewModel?> GetStaffViewModelByIdAsync(Guid staffId);
        Task<Guid?> UpsertStaffAsync(StaffRequest request, Guid ownerId);
        Task<bool> DeleteStaffAsync(Guid staffId);
    }
}
