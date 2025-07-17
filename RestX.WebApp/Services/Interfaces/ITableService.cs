using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Interfaces
{
    public interface ITableService
    {
        public Task<Table> GetTableByIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<TableStatusViewModel>> GetAllTablesByOwnerIdAsync(Guid? guid, CancellationToken cancellationToken = default);
        public Task<List<TableStatusViewModel>> GetAllTablesByCurrentStaff(CancellationToken cancellationToken = default);
        public Task<TableStatusViewModel> UpdateTableStatusAsync(int tableId, int newStatusId);
    }
}
