using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HIS.Web.UI 
{
    public class DataController : ApiController
    {
        private IRepositoryFactory factory;

        public DataController(IRepositoryFactory factory) {

            this.factory = factory;
        }

        [HttpPost]
        public dynamic GetUsers(dynamic filter) {

            var repository = factory.GetRepository<Repository<Users>>();

            var query = repository.GetQueryable();

            var oList = 
                    query.Select(o=>
                    new 
                    {
                       Username = o.UserName,
                       Email = o.Email

                    }).ToList();

           

            return oList;
        }
    }
}