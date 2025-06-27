using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Controllers
{
    public class OwnerController : BaseController
    {
        public OwnerController(IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
        }

        [HttpGet]
        [Route("Owner/Index/{ownerId:guid}")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }


        [HttpGet("Owner/Dishes")]
        public async Task<IActionResult> DishesManagement()
        {
            // Tạo dữ liệu mẫu cho CategoryViewModel
            var categories = new List<CategoryViewModel>
    {
        new CategoryViewModel { Id = 1, CategoryName = "Appetizer" },
        new CategoryViewModel { Id = 2, CategoryName = "Main Course" },
        new CategoryViewModel { Id = 3, CategoryName = "Dessert" }
    };

            // Tạo dữ liệu mẫu cho DishesViewModel
            var dishes = new List<DishesViewModel>
    {
        new DishesViewModel
        {
            Id = 1,
            Name = "Spring Rolls",
            Description = "Crispy rolls with vegetables",
            Price = 5.99m,
            ImageUrl = "/images/dishes/springrolls.jpg",
            CategoryName = "Appetizer",
            IsActive = true
        },
        new DishesViewModel
        {
            Id = 2,
            Name = "Grilled Chicken",
            Description = "Juicy grilled chicken breast",
            Price = 12.50m,
            ImageUrl = "/images/dishes/grilledchicken.jpg",
            CategoryName = "Main Course",
            IsActive = true
        },
        new DishesViewModel
        {
            Id = 3,
            Name = "Chocolate Cake",
            Description = "Rich chocolate layered cake",
            Price = 6.75m,
            ImageUrl = "/images/dishes/chocolatecake.jpg",
            CategoryName = "Dessert",
            IsActive = false
        }
    };

            var model = new DishesManagementViewModel
            {
                Dishes = dishes,
                Categories = categories
            };

            return View(model);
        }

    }
}
