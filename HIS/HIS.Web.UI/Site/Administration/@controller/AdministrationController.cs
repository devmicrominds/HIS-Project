using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HIS.Web.UI
{
    public class AdministrationController : ApiController
    {
        private IRepositoryFactory factory;

        public AdministrationController(IRepositoryFactory factory)   {

            this.factory = factory;
        }

        [HttpPost]
        public bool SaveUser(dynamic data)
        {
            var roleId = Guid.Parse((string)data.role);
            var rolesQuery = factory.GetRepository<Repository<Roles>>().GetQueryable();
            var role = rolesQuery.FirstOrDefault(o => o.Id == roleId);

            var User = new Users()
            {
                UserName = data.username,
                Email = data.email,
                UserPassword = data.userpassword,
                UserRoles = role,

            };

            try
            {
                factory.OnTransaction(() =>
                {
                    var repository = factory.GetRepository<Repository<Users>>();

                    repository.SaveOrUpdate(User);



                });

            }
            catch (Exception ex)
            {

            }

            return false;
        }

        [HttpGet]
        public dynamic GetUserRoles()
        {

            var repository = factory.GetRepository<Repository<Roles>>();
            var query = repository.GetQueryable();

            var result = new List<dynamic>() 
            { 
                new {
                    value = 0,
                    text = "",
                }
            
            };

            result.AddRange(query.Select(o =>
                                  new
                                  {
                                      value = o.Id,
                                      text = o.Name
                                  }));


            return result.ToArray();
        }

        [HttpGet]
        public dynamic Privileges(int sEcho,
        int iDisplayStart,
        int iDisplayLength,
        string sSearch,
        int iSortCol_0,
        string sSortDir_0)
        {
            return new
            {
                sEcho = sEcho,
                iTotalRecords = 100,
                iTotalDisplayRecords = 100,
                aaData = new[]
            {
                new[]
                {
                    "Column 1",
                    "Column 2",
                    "Column 3"
                    // ...
                }  ,

                 new[]
                {
                    "Column 1",
                    "Column 2",
                    "Column 3"
                    // ...
                }
            }
            };

        }
         
    }
}