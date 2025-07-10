using RestX.WebApp.Models.ViewModels;

namespace RestX.WebApp.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerViewModel>> GetCustomersByOwnerIdAsync(Guid ownerId);
        Task<CustomerViewModel?> GetCustomerByIdAsync(Guid id);
        Task<Guid?> UpsertCustomerAsync(CustomerViewModel model, Guid ownerId);
        Task<bool> DeleteCustomerAsync(Guid id);
    }
}
