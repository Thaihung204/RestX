using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        public CustomerService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
        }

        public List<Customer> GetCustomers()
        {
            return Repo.GetAll<Customer>().ToList();

        }
    }
}
