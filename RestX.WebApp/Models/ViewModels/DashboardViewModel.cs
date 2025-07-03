namespace RestX.WebApp.Models.ViewModels
{
    public class DailyFinanceViewModel
    {
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }      
        public decimal Profit { get; set; }   
    }

    public class DashboardViewModel
    {
        public List<DailyFinanceViewModel> DailyFinances { get; set; } = new();
    }
}
