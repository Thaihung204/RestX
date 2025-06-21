using RestX.WebApp.Models;
using RestX.WebApp.Models.Home;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class HomeService : BaseService, IHomeService
    {
        private readonly ICustomerService customerService;
        private readonly IOwnerService ownerService;

        public HomeService(ICustomerService customerService, IOwnerService ownerService, IRepository repo) : base(repo)
        {
            this.customerService = customerService;
            this.ownerService = ownerService;
        }

        public async Task<HomeViewModel> GetHomeViewsAsync(Guid ownerId, int tableId, CancellationToken cancellationToken = default)
        {
            var owner = await ownerService.GetOwnerByIdAsync(ownerId);

            if (owner == null)
                throw new Exception($"Không tìm thấy owner với ID: {ownerId}");

            var homeViewModel = new HomeViewModel
            {
                Id = owner.Id,
                FileId = owner.FileId,
                Name = owner.Name ?? string.Empty,
                Address = owner.Address ?? string.Empty,
                FileName = owner.File?.Name ?? "Defaul",
                FileUrl = owner.File?.Url ?? "/images/default.png"
            };

            return homeViewModel;
        }
    }
}
