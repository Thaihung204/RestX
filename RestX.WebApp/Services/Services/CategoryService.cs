using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await repo.GetAllAsync<Category>();
            return categories.ToList();
        }
    }
}
