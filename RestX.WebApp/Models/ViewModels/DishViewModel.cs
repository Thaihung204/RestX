
namespace RestX.WebApp.Models.ViewModels
{
    public class DishViewModel : IOwnerViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsActive { get; set; }
        public Guid? OwnerId { get; set; }
    }
}