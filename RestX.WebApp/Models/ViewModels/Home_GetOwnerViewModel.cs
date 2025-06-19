namespace RestX.WebApp.Models.ViewModels
{
    public class Home_GetOwnerViewModel
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;
        public string? FileName { get; set; } = null!;
        public string? FileUrl { get; set; } = null!;

    }
}
