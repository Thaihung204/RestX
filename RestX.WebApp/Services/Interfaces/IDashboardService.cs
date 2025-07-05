using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestX.WebApp.Models.ViewModels;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<Dictionary<DateTime, decimal>> GetCostByDateAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<Dictionary<DateTime, decimal>> GetProfitByDateAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<DashboardViewModel> GetDashboardViewModelAsync(Guid ownerId, CancellationToken cancellationToken = default);
    }
}
