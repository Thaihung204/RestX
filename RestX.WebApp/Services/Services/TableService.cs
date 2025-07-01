using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class TableService : BaseService, ITableService
    {
        public TableService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
        }

        public async Task<Table> GetTableByIdAsync(int id)
        {
            return await Repo.GetOneAsync<Table>(t => t.Id == id);
        }
    }
}
