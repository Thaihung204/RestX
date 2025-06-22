using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class OwnerService : BaseService, IOwnerService
    {
        public OwnerService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
        }

        public async Task<Owner> GetOwnerByIdAsync(Guid id)
        {
            return await repo.GetOneAsync<Owner>(o => o.Id == id, "File");
        }
    }
}
