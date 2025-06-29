using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class TableService : BaseService, ITableService
    {
        public TableService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
        }

        public async Task<Table> GetTableByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await repo.GetOneAsync<Table>(t => t.Id == id);
        }

        public async Task<List<TableStatusViewModel>> GetAllTablesByOwnerIdAsync(Guid? guid, CancellationToken cancellationToken = default)
        {
            var tables = await repo.GetAsync<Table>(
                filter: t => t.OwnerId == guid,
                orderBy: q => q.OrderBy(t => t.TableNumber),
                includeProperties: "TableStatus"
            );

            var tableViewModels = tables.Select(t => new TableStatusViewModel
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                TableStatus = t.TableStatus,
            });
            return tableViewModels.ToList();
        }

        public async Task<TableStatusViewModel> UpdateTableStatusAsync(int tableId, int newStatusId)
        {
            var table = await repo.GetByIdAsync<Table>(tableId);
            if (table == null)
            {
                return null;
            }

            table.TableStatusId = newStatusId;
            repo.Update(table);
            await repo.SaveAsync();

            var updatedTable = await repo.GetOneAsync<Table>(
                filter: t => t.Id == tableId,
                includeProperties: "TableStatus"
            );

            return new TableStatusViewModel
            {
                Id = updatedTable.Id,
                TableNumber = updatedTable.TableNumber,
                TableStatus = updatedTable.TableStatus
            };
        }
    }
}
