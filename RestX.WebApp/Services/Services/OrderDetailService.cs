using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace RestX.WebApp.Services.Services
{
    public class OrderDetailService : BaseService, IOrderDetailService
    {
        public OrderDetailService(IRepository Repo, IHttpContextAccessor httpContextAccessor)
            : base(Repo, httpContextAccessor)
        {
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var orderDetails = await Repo.GetAsync<OrderDetail>(od => od.Order.OwnerId == ownerId, includeProperties: "Order");
            return orderDetails.ToList();
        }
    }
}