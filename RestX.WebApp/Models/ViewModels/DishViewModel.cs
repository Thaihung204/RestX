
using System.ComponentModel.DataAnnotations;

namespace RestX.WebApp.Models.ViewModels
{
    public class DishViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsActive { get; set; }
    }
    public class DishRequest
    {
        public int? Id { get; set; } // Thêm Id property

        [Required(ErrorMessage = "Dish name is required")]
        [MaxLength(100, ErrorMessage = "Dish name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        // Không required - sẽ validate trong controller
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = true;
    }
}