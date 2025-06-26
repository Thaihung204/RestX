namespace RestX.WebApp.Models.ViewModels
{
    public class DishesManagementViewModel
    {
        public List<DishesViewModel> Dishes { get; set; } = new();
        public List<CategoryViewModel> Categories { get; set; } = new();
    }

    public class DishesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public bool? IsActive { get; set; }
    }
}

