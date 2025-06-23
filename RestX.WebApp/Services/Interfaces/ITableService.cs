using RestX.WebApp.Models;

namespace RestX.WebApp.Services.Interfaces
{
    public interface ITableService
    {
        public Task<Table> GetTableByIdAsync(int id);

    }
}
