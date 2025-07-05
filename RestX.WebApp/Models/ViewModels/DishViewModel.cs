
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
    // New DTO for Create operations
    public class CreateDishRequest
    {
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

        [Required(ErrorMessage = "Image file is required")]
        public IFormFile ImageFile { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }

    // New DTO for Edit operations
    public class EditDishRequest
    {
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

        // Optional for edit - keep existing image if not provided
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = true;
    }
}