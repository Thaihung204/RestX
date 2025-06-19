using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class BaseService
    {
        public readonly IRepository Repo;
        //public IRedisService RedisService;
        //public readonly ActiveTenant CurrentTenant;

        public BaseService(IRepository repo
            //IRedisService redisService, 
            //IEnumerable<ActiveTenant> tenant = null
            )
        {
            this.Repo = repo;
            //this.RedisService = redisService;
            //this.CurrentTenant = tenant?.FirstOrDefault();
        }
    }
}
