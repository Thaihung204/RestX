using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IRepository repo) : base(repo) { }

        public List<Category> GetCategories()
        {
            return Repo.GetAll<Category>().ToList();
        }
    }
}
