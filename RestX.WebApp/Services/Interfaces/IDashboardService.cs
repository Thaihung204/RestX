using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestX.WebApp.Models.ViewModels;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<Dictionary<DateTime, decimal>> GetCostByDateAsync(CancellationToken cancellationToken = default);
        Task<Dictionary<DateTime, decimal>> GetProfitByDateAsync(CancellationToken cancellationToken = default);
        Task<DashboardViewModel> GetDashboardViewModelAsync(CancellationToken cancellationToken = default);
    }
}
