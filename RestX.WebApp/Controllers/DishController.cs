using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Controllers
{
    public class DishController : Controller
    {
        private readonly IRepository _repo;

        public DishController(IRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Menu()
        {
            var categories = _repo.GetAll<Models.Category>(
                    orderBy: q => q.OrderBy(c => c.Name),
                    includeProperties: "Dishes,Dishes.File"
                )
                .Select(category => new CategoryViewModel
                {
                    Id = category.Id,
                    CategoryName = category.Name,
                    Dishes = category.Dishes
                        .OrderBy(d => d.Name)
                        .Select(dish => new DishViewModel
                        {
                            Id = dish.Id,
                            Name = dish.Name,
                            Description = dish.Description,
                            Price = dish.Price,
                            ImageUrl = dish.File?.Url ?? "/images/no-image.png"
                        }).ToList()
                }).ToList();

            var model = new MenuViewModel
            {
                Categories = categories
            };

            return View(model);
        }
    }
}
