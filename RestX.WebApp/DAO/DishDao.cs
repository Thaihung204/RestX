using RestX.WebApp.Models;

namespace RestX.WebApp.DAO
{
    public class DishDao
    {
        private readonly RestXRestaurantManagementContext _context;
        private readonly ILogger<DishDao> _logger;

        public DishDao(RestXRestaurantManagementContext context, ILogger<DishDao> logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
