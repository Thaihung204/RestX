using RestX.WebApp.Models;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IDishService
    {
        Task<List<Dish>> GetDishesAsync();
        Task<List<Dish>> GetDishesByOwnerIdAsync(Guid ownerId);
    }
}