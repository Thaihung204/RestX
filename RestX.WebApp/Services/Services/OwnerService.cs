using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class OwnerService : BaseService, IOwnerService
    {
        public OwnerService(IRepository Repo, IHttpContextAccessor httpContextAccessor) : base(Repo, httpContextAccessor)
        {
        }

        public async Task<Owner> GetOwnerByIdAsync(Guid id)
        {
            return await Repo.GetOneAsync<Owner>(o => o.Id == id, "File");
        }
    }
}
