using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class DishService : BaseService, IDishService
    {
        public DishService(IRepository repo) : base(repo)
        {
        }

        public List<Models.Dish> GetDishes()
        {
            return Repo.GetAll<Models.Dish>().ToList();
           
        }
    }
}
