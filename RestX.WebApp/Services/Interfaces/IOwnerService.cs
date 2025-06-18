using RestX.WebApp.Models;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IOwnerService
    {
        public Task<Owner> GetOwnerByIdAsync(Guid id);
    }
}
