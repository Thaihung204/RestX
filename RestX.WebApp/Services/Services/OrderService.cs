using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(IRepository repo, IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
        }

        public async Task<List<Order>> GetOrdersByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var orders = await repo.GetAsync<Order>(o => o.OwnerId == ownerId);
            return orders.ToList();
        }
    }
}