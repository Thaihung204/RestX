using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class MenuService : BaseService, IMenuService
    {
        private readonly IDishService _dishService;
        private readonly ICategoryService _categoryService;

        public MenuService(IRepository repo, IDishService dishService, ICategoryService categoryService, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
            _dishService = dishService;
            _categoryService = categoryService;
        }

        public async Task<MenuViewModel> GetMenuViewModelAsync(CancellationToken cancellationToken = default)
        {
            var dishes = await _dishService.GetDishesByOwnerIdAsync(OwnerId);
            var categories = await _categoryService.GetCategoriesAsync();

            var model = new MenuViewModel
            {
                ownerId = OwnerId,
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

            return model;
        }
    }
}
