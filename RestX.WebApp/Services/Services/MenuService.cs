using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Services
{
    public class MenuService : BaseService, IMenuService
    {
        private readonly IDishService _dishService;
        private readonly ICategoryService _categoryService;

        public MenuService(IRepository repo, IDishService dishService, ICategoryService categoryService) : base(repo)
        {
            _dishService = dishService;
            _categoryService = categoryService;
        }

        public async Task<MenuViewModel> GetMenuViewModelAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var dishes = _dishService.GetDishes()
                .Where(d => d.IsActive == true && d.OwnerId == ownerId)
                .ToList();
            var categories = _categoryService.GetCategories();

            var model = new MenuViewModel
            {
                ownerId = ownerId,
                Categories = categories
                    .OrderBy(c => c.Name)
                    .Select(category => new CategoryViewModel
                    {
                        Id = category.Id,
                        CategoryName = category.Name,
                        Dishes = dishes
                            .Where(d => d.CategoryId == category.Id)
                            .OrderBy(d => d.Name)
                            .Select(dish => new DishViewModel
                            {
                                Id = dish.Id,
                                Name = dish.Name,
                                Description = dish.Description,
                                Price = dish.Price,
                                ImageUrl = dish.File?.Url ?? "/images/no-image.png"
                            }).ToList()
                    }).ToList()
            };

            return await Task.FromResult(model);
        }
    }
}
