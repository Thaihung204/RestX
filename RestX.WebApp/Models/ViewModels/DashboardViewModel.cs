namespace RestX.WebApp.Models.ViewModels
{
    public class DailyFinanceViewModel
    {
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }      
        public decimal Profit { get; set; }   
    }

    public class DashboardViewModel : IOwnerViewModel
    {
        public Guid? OwnerId { get; set; }
        public List<DailyFinanceViewModel> DailyFinances { get; set; } = new();
    }
}
