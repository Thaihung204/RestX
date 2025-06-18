using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        public CustomerService(IRepository repo) : base(repo)
        {
        }

        public List<Models.Customer> GetCustomers()
        {
            return Repo.GetAll<Models.Customer>().ToList();

        }
    }
}
