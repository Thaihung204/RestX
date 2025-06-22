using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class DishService : BaseService, IDishService
    {
        public DishService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
        }

        public List<Dish> GetDishes()
        {
            Console.WriteLine($"OwnerId: {OwnerId}"); // Test xem có lấy được OwnerId không
            Console.WriteLine($"TableId: {TableId}"); // Test xem có lấy được TableId không

            return repo.GetAll<Dish>().ToList();
        }
    }
}
