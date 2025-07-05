using RestX.WebApp.Models;

namespace RestX.WebApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<int> CreateCategoryAsync(string categoryName, string userId);
        Task<Category?> GetCategoryByNameAsync(string name);
    }
}
