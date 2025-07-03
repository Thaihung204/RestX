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
    public class IngredientImportService : BaseService, IIngredientImportService
    {
        public IngredientImportService(IRepository repo, IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
        }

        public async Task<List<IngredientImport>> GetIngredientImportsByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var imports = await repo.GetAsync<IngredientImport>(i => i.Ingredient.OwnerId == ownerId, includeProperties: "Ingredient");
            return imports.ToList();
        }
    }
}