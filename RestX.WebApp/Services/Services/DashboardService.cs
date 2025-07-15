using Microsoft.AspNetCore.Http;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Services
{
    public class DashboardService : BaseService, IDashboardService
    {
        private readonly IOrderDetailService orderDetailService;
        private readonly IIngredientImportService ingredientImportService;

        public DashboardService(
            IRepository repo,
            IHttpContextAccessor httpContextAccessor,
            IOrderDetailService orderDetailService,
            IIngredientImportService ingredientImportService
        ) : base(repo, httpContextAccessor)
        {
            this.orderDetailService = orderDetailService;
            this.ingredientImportService = ingredientImportService;
        }

        public async Task<Dictionary<DateTime, decimal>> GetCostByDateAsync(CancellationToken cancellationToken = default)
        {
            var imports = await ingredientImportService.GetIngredientImportsByOwnerIdAsync(cancellationToken);
            return imports
                .Where(i => i.Time.HasValue)
                .GroupBy(i => i.Time.Value.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(i => i.TotalCost)
                );
        }

        public async Task<Dictionary<DateTime, decimal>> GetProfitByDateAsync(CancellationToken cancellationToken = default)
        {
            var orderDetails = await orderDetailService.GetOrderDetailsByOwnerIdAsync(cancellationToken);
            return orderDetails
                .Where(od => od.Order != null && od.Order.Time.HasValue)
                .GroupBy(od => od.Order.Time.Value.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(od => od.Quantity * od.Price)
                );
        }

        public async Task<DashboardViewModel> GetDashboardViewModelAsync(CancellationToken cancellationToken = default)
        {
            var profitByDate = await GetProfitByDateAsync(cancellationToken);

            var dailyFinances = profitByDate.OrderBy(x => x.Key)
                .Select(x => new DailyFinanceViewModel
                {
                    Date = x.Key,
                    Profit = x.Value
                }).ToList();

            return new DashboardViewModel
            {
                DailyFinances = dailyFinances
            };
        }
    }
}
