namespace RestX.WebApp.Models.Home
{
    public class HomeViewModel
    {
        public Guid OwnerId { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? FileName { get; set; } = null!;
        public string? FileUrl { get; set; } = null!;
        public int TableNumber { get; set; }
        }
}