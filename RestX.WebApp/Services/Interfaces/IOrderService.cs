using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestX.WebApp.Models;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    }
}