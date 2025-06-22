namespace RestX.WebApp.Models.ViewModels
{
    public class MenuViewModel
    {
        public Guid ownerId { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new();
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }    
        public string CategoryName { get; set; } = null!;
        public List<DishViewModel> Dishes { get; set; } = new();
    }

    public class DishViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
