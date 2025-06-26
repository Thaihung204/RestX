using Microsoft.EntityFrameworkCore;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class AuthCustomerService : BaseService, IAuthCustomerService
    {
        public AuthCustomerService(IRepository repo, IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
        }

        public async Task<Customer> LoginOrCreateAsync(LoginViewModel model, CancellationToken cancellationToken = default)
        {
            ValidateLoginModel(model);

            var customer = await FindExistingCustomerAsync(model.Phone, model.OwnerId, cancellationToken);

            if (customer != null)
            {
                return await UpdateExistingCustomerAsync(customer, model, cancellationToken);
            }

            return await CreateNewCustomerAsync(model, cancellationToken);
        }

        private static void ValidateLoginModel(LoginViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.Phone))
                throw new ArgumentException("Số điện thoại không được để trống", nameof(model.Phone));

            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentException("Tên không được để trống", nameof(model.Name));

            if (model.OwnerId == Guid.Empty)
                throw new ArgumentException("Owner ID không hợp lệ", nameof(model.OwnerId));
        }

        private async Task<Customer?> FindExistingCustomerAsync(string phone, Guid ownerId, CancellationToken cancellationToken)
        {
            return await repo.GetFirstAsync<Customer>(
                c => c.Phone == phone && c.OwnerId == ownerId
            );
        }

        private async Task<Customer> UpdateExistingCustomerAsync(Customer customer, LoginViewModel model, CancellationToken cancellationToken)
        {
            if (customer.Name != model.Name)
            {
                customer.Name = model.Name;
                customer.ModifiedDate = DateTime.UtcNow;

                await repo.UpdateAsync(customer, cancellationToken);
            }

            return customer;
        }

        private async Task<Customer> CreateNewCustomerAsync(LoginViewModel model, CancellationToken cancellationToken)
        {
            var newCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                OwnerId = model.OwnerId,
                Name = model.Name,
                Phone = model.Phone,
                Point = 0,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            };

            await repo.CreateAsync(newCustomer, cancellationToken);
            return newCustomer;
        }
    }
}