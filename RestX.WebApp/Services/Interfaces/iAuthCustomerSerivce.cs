using RestX.WebApp.Models;

using RestX.WebApp.Models.ViewModels;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Interfaces
{
    // <-- ĐÃ THAY ĐỔI TÊN INTERFACE
    public interface IAuthCustomerService
    {
        // <-- ĐÃ THAY ĐỔI KIỂU TRẢ VỀ
        Task<Customer> LoginOrCreateAsync(LoginViewModel model, CancellationToken cancellationToken = default);
    }
}