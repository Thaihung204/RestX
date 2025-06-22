using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Controllers
{
    public class DishController : Controller
    {
        private readonly IRepository _repo;
        private readonly IDishService dishService;

        public DishController(IRepository repo, IDishService dishService)
        {
            _repo = repo;
            this.dishService = dishService;
        }

        [HttpGet]
        [Route("Dish/Menu/{ownerId:guid}/{tableId:int}")]
        public IActionResult Menu(Guid ownerId, int tableId)
        {
            dishService.GetDishes(); // Test xem có lấy được OwnerId không

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
