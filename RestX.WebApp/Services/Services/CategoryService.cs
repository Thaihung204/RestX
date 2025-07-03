using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IRepository Repo, IHttpContextAccessor httpContextAccessor) : base(Repo, httpContextAccessor)
        {
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await Repo.GetAllAsync<Category>();
            return categories.ToList();
        }

        public async Task<int> CreateCategoryAsync(string categoryName, string userId)
        {
            var existingCategory = await GetCategoryByNameAsync(categoryName);
            if (existingCategory != null)
            {
                return existingCategory.Id;
            }

            var category = new Category
            {
                Name = categoryName.Trim()
            };

            var result = await Repo.CreateAsync(category, userId);
            await Repo.SaveAsync();
            return (int)result;
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            var categories = await Repo.GetAsync<Category>(c => c.Name.ToLower() == name.ToLower().Trim());
            return categories.FirstOrDefault();
        }
    }
}