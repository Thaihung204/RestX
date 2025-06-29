namespace RestX.WebApp.Models.ViewModels
{
    public class DishesManagementViewModel : IOwnerViewModel
    {
        public Guid? OwnerId { get; set; }
        public List<DishViewModel> Dishes { get; set; } = new();
    }
}

