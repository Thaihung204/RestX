using Microsoft.EntityFrameworkCore;
using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class LoginService : BaseService, ILoginService
    {
        public LoginService(IRepository repo) : base(repo)
        {
        }

        public Task<Account> GetAccountByUsernameAsync(string username, string password, CancellationToken cancellationToken)
        {
            return Repo.GetFirstAsync<Account>(
                filter: acc => acc.Username == username && acc.Password == password,
                includeProperties: "Staff,Owner"
                );
        }
    }
}
