namespace RestX.WebApp.Models.Home
{
    public class HomeViewModel
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? FileName { get; set; } = null!;
        public string? FileUrl { get; set; } = null!;
    }
}