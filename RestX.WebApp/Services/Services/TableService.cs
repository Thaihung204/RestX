using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Helper;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System;

namespace RestX.WebApp.Services.Services
{
    public class TableService : BaseService, ITableService
    {
        private readonly IMapper mapper;

        public TableService(IRepository repo, IHttpContextAccessor httpContextAccessor, IMapper mapper)
            : base(repo, httpContextAccessor)
        {
            this.mapper = mapper;
        }

        public async Task<List<TableStatus>> GetTableStatus()
        {
            var statuses = await Repo.GetAsync<TableStatus>();
            return statuses.ToList();
        }

        public async Task<Table> GetTableByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await Repo.GetOneAsync<Table>(t => t.Id == id);
        }

        public async Task<TableViewModel> GetTableViewModelByIdAsync(int id)
        {
            return mapper.Map<TableViewModel>(GetAllTablesAsync());
        }

        public async Task<List<TableStatusViewModel>> GetAllTablesByOwnerIdAsync(Guid? guid, CancellationToken cancellationToken = default)
        {

            var tables = await Repo.GetAsync<Table>(
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
            var table = await Repo.GetByIdAsync<Table>(tableId);
            if (table == null)
            {
                return null;
            }

            table.TableStatusId = newStatusId;
            Repo.Update(table);
            await Repo.SaveAsync();

            var updatedTable = await Repo.GetOneAsync<Table>(
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

        public async Task<TableListViewModel> GetAllTablesAsync()
        {
            var ownerId = UserHelper.GetCurrentOwnerId();
            var tables = await Repo.GetAsync<Table>(
                filter: t => t.OwnerId == ownerId,
                orderBy: q => q.OrderBy(t => t.TableNumber),
                includeProperties: "TableStatus"
            );
            var tableViewModels = tables.Select(t => mapper.Map<TableViewModel>(t)).ToList();
            return new TableListViewModel { Tables = tableViewModels };
        }

        public async Task<int?> UpsertTableAsync(TableViewModel model)
        {
            var ownerId = UserHelper.GetCurrentOwnerId();
            Table table;
            
            if (model.Id != 0)
            {
                table = await Repo.GetByIdAsync<Table>(model.Id);
                if (table == null) return null;
                mapper.Map(model, table);
                table.OwnerId = ownerId;
                Repo.Update(table);
            }
            else
            {
                table = mapper.Map<Table>(model);
                table.OwnerId = ownerId;
                await Repo.CreateAsync(table);
            }
            await Repo.SaveAsync();
            return table.Id;
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            var table = await Repo.GetByIdAsync<Table>(id);
            if (table == null) return false;
            Repo.Delete<Table>(id);
            await Repo.SaveAsync();
            return true;
        }
    }
}
