using AutoMapper;
using RestX.BLL.Interfaces;

namespace RestX.BLL.Services
{
    public class Class1 : BaseService
    {
        private readonly IMapper mapper;

        public Class1(IMapper mapper, IRepository repo)
        {
            this.mapper = mapper;
        }

        #region CRUD

        #endregion
    }
}
