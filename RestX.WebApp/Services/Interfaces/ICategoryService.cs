using RestX.WebApp.Models;

namespace RestX.WebApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
    }
}
