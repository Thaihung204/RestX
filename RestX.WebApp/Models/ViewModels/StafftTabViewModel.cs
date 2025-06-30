namespace RestX.WebApp.Models.ViewModels
{
    public class StafftTabViewModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Guid? OwnerId { get; set; }

        public bool? IsActive { get; set; }
    }
}
