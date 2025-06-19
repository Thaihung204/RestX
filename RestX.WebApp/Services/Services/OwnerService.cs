using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class OwnerService : BaseService, IOwnerService
    {
        private readonly IRepository repo;

        public OwnerService(IRepository repo) : base(repo)
        {
            this.repo = repo;
        }

        public async Task<Owner> GetOwnerByIdAsync(Guid id)
        {
            // Sử dụng include để lấy cả File
            return await repo.GetOneAsync<Owner>(o => o.Id == id, "File");
        }
    }
}
