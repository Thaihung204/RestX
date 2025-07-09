using RestX.WebApp.Models;
using RestX.WebApp.Models.DTO;

namespace RestX.WebApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<int> CreateCategoryAsync(CategoryDto request, Guid ownerId);
        Task<Category?> GetCategoryByNameAsync(string name);
    }
}
