using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Controllers
{
    public class DishController : Controller
    {
        private readonly IDishService _dishService;
        private readonly ICategoryService _categoryService;

        public DishController(IDishService dishService, ICategoryService categoryService)
        {
            _dishService = dishService;
            _categoryService = categoryService;
        }

        public IActionResult Menu()
        {
            var dishes = _dishService.GetDishes();
            var categories = _categoryService.GetCategories();

            var model = new MenuViewModel
            {
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

            return View(model);
        }
    }
}
