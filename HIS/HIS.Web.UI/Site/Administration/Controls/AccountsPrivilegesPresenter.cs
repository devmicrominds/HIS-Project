using HIS.Data;
using HIS.DataAccess;
using HIS.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace HIS.Web.UI
{
    /// <summary>
    /// Todo : define action
    /// </summary>
    public class AccountsPrivilegesPresenter : GenericPresenter<IAccountsPrivilegesView>
    {
        IRepositoryFactory factory;
        const string userlist_page_key = "accounts.privileges.list";
        private IDictionary<string, Action> RunAction;


        public AccountsPrivilegesPresenter(IRepositoryFactory factory)
        {

            this.factory = factory;
            BuildRunActions();
        }


        public override void View_Load(object sender, EventArgs e)
        {

            if (!PageSettingsList.ContainsKey(userlist_page_key))
                PageSettingsList[userlist_page_key] = PageSettings.Default();
            var p = View.PostParameter;
            var data = p.Data;
            // Guid guid = p.Data.Role;
            //Guid guid = data == null ? data : data.Role;
            View.JsonData = Json(new
            {
                Roles = Roles(),
                SelectedRole = data == null ? data : data.Role
            });

            string action = View.PostParameter.Action;

            if (RunAction.ContainsKey(action))
            {
                RunAction[action]();
            }


            if (data != null)
            {
                View.Model = GetPagedUsers();
                string role = data.Role;
                BindGridView(role);

            }



        }


        private void BuildRunActions()
        {
            RunAction = new Dictionary<string, Action>() 
            {
                 
                 { "UpdatePrivilege",UpdatePrivilege },
                 
            };
        }



        private PagedResult<Privileges> GetPagedUsers()
        {
            var e = View.PostParameter;

            var repository = factory.GetRepository<Repository<Privileges>>();
            var query = repository.GetQueryable();
            // filter here
            return repository.PagedResult(query, PageSettingsList[userlist_page_key].PageIndex, PageSettingsList[userlist_page_key].PageSize);


        }



        private void BindGridView(string roleId)
        {
            #region OLD
            /*
            var repository = factory.GetRepository<Repository<Roles>>();
            var query = repository.GetQueryable();
            Guid guid01 = new Guid(role);


            var oPrivilageAll = new List<RolesPrivilegeViewModel>();


            query.ToList().ForEach(x =>
            {
                x.RolePrivileges.ToList().ForEach(y =>
                {
                    bool status = false;

                    if (x.Id == guid01)
                    {
                        status = true;
                    }

                    oPrivilageAll.Add(new RolesPrivilegeViewModel()
                    {
                        RoleId = x.Id,
                        checkedId = "",
                        Accessable = status,
                        RoleDesc = y.PrivilegeDesc
                    });
                });

            });
             */
            #endregion
            Guid guid = Guid.Empty;

            Guid.TryParse(roleId, out guid);

            var roles = GetRoleById(guid);


            var repository = factory.GetRepository<Repository<Privileges>>();

            var query = repository.GetQueryable();

            var privList = query
                                .Select(o => new RolesPrivilegeViewModel()
                                {
                                    PrivilegeId = o.Id,
                                    PrivilegeDesc = o.PrivilegeDesc,
                                    Accessibility = false,
                                    RoleId = guid,
                                }).ToList();

            privList.ForEach(o =>
            {

                if (roles.RolePrivileges.Any(r => r.Id == o.PrivilegeId))
                    o.Accessibility = true;

            });


            View.LocalPageSettings = PageSettingsList[userlist_page_key];
            View.MainGrid.VirtualItemCount = privList.ToList().Count;
            View.MainGrid.DataSource = privList;
            View.MainGrid.DataBind();


        }

        private void UpdatePrivilege()
        {
            var p = View.PostParameter;
            var data = p.Data;
            string roleid = data.Role;
            string privId = data.PrivillageID;


            Guid privIds = new Guid(privId);
            Guid rolesids = new Guid(roleid);




            var role = GetRoleById(rolesids);

            bool exists = role.RolePrivileges.ToList().Exists(x => x.Id == privIds);

            var priv = GetPrivilegeById(privIds);

            factory.OnTransaction(() =>
            {
                if (!exists)
                    role.AddPrivilege(priv);
                else
                    role.RemovePrivilege(priv);



            });


        }


        private Privileges GetPrivilegeById(Guid privID)
        {

            var repository = factory.GetRepository<Repository<Privileges>>();
            var query = repository.GetQueryable();

            return query.Where(o => o.Id == privID).FirstOrDefault();
        }


        private Roles GetRoleById(Guid roleId)
        {

            var repository = factory.GetRepository<Repository<Roles>>();
            var query = repository.GetQueryable();

            return query.Where(o => o.Id == roleId).FirstOrDefault();
        }

        private dynamic Roles()
        {

            var repository = factory.GetRepository<Repository<Roles>>();

            var query = repository.GetQueryable();

            var list = new List<object>();

            list.Add(new { title = @"Role  ", value = "" });

            list.AddRange(query.Select(o => new { title = o.Name, value = o.Id }));

            return list.ToArray();

        }



    }


}