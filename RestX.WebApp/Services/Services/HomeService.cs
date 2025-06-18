using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class HomeService : BaseService, IHomeService
    {
        public HomeService(IRepository repo) : base(repo)
        {
        }

    }
}
