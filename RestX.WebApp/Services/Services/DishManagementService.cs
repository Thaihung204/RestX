using AutoMapper;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class DishManagementService : BaseService, IDishManagementService
    {
        private readonly IDishService dishService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public DishManagementService(
            IRepository repo,
            IDishService dishService,
            ICategoryService categoryService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
            : base(repo, httpContextAccessor)
        {
            this.dishService = dishService;
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        public async Task<DishesManagementViewModel> GetDishesManagementViewModelAsync(Guid ownerId)
        {
            var dishes = await dishService.GetDishesByOwnerIdAsync(ownerId);
            var categories = await categoryService.GetCategoriesAsync();

            return new DishesManagementViewModel
            {
                Dishes = mapper.Map<List<DishViewModel>>(dishes),
                Categories = categories
            };
        }
    }
}