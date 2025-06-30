namespace RestX.WebApp.Models.ViewModels
{
    public class StaffManagementViewModel : IOwnerViewModel
    {
        public Guid? OwnerId { get; set; }
        public List<StafftTabViewModel> Staffs { get; set; } = new List<StafftTabViewModel>();
    }
}