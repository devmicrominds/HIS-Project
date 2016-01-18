using Newtonsoft.Json;
using HIS.Data;
using HIS.DataAccess;
using HIS.Web.UI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace HIS.Web.UI
{
    public class AccountsPresenter : GenericPresenter<IAccountsView>
    {

        private IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;

        public AccountsPresenter(IRepositoryFactory factory)
        {

            this.factory = factory;
            BuildRunActions();
        }

        /// <summary>
        /// register page actions
        /// </summary>
        private void BuildRunActions()
        {
            RunAction = new Dictionary<string, Action>() 
            {
                 { "PageSettingsChanged", PageSettingsChanged },
                 { "ShowTab",ProcessShowTab },
                 { "SaveUser",ProcessSaveUser },
                 { "SaveRoles",ProcessSaveRoles },
                 { "FindRoles",FindSpecificRoles },
                 { "UpdatePrivilege",UpdatePrivilege },
                 { "EditUserGrid",ProcessEditGrid },
                 { "EditRolesGrid",ProcessRolesGrid },
            };

        }

        public override void InitialRun()
        {
            base.InitialRun();
            View.AddControl(ControlPath.IAccountsUsersView, View.ControlPlaceHolder);

        }

        public override void PartialRender()
        {
            base.PartialRender(); // a must for page action
            string action = View.PostParameter.Action;

            if (RunAction.ContainsKey(action))
            {
                RunAction[action]();
            }


        }

        private void UpdatePrivilege()
        {

            View.AddControl(ControlPath.IAccountsPrivilegesView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }

        private void FindSpecificRoles()
        {

            var p = View.PostParameter;
            var data = p.Data;
            View.AddControl(ControlPath.IAccountsPrivilegesView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);

        }

        private void ProcessSaveUser()
        {

            var p = View.PostParameter;
            var data = p.Data;

            string idU = data.ID;
            string roleGuid = data.Role;
            bool delete = data.Operation;

            Guid userids = idU == null ? Guid.Parse("00000000-0000-0000-0000-000000000000") : Guid.Parse(idU);

            var repository = factory.GetRepository<Repository<Users>>();
            var query = repository.GetQueryable();
            var oUser = query.Where(o => o.Id == userids).FirstOrDefault();

            if (!delete)
            {
                if (oUser == null)
                {
                    oUser = new Users()
                    {
                        UserName = data.Username,
                        Email = data.Email,
                        UserPassword = data.Password,
                        UserRoles = GetRoleById(Guid.Parse(roleGuid))

                    };
                }
                else
                {
                    oUser.UserName = data.Username;
                    oUser.Email = data.Email;
                    oUser.UserPassword = data.Password;
                    oUser.UserRoles = GetRoleById(Guid.Parse(roleGuid));

                }


                factory.OnTransaction(() =>
                {
                    oUser = repository.SaveOrUpdate(oUser);
                });
            }
            else
            {
                factory.OnTransaction(() =>
                {

                    repository.Delete(oUser);

                });
            }
            View.AddControl(ControlPath.IAccountsUsersView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }

        private void ProcessSaveRoles()
        {

            var p = View.PostParameter;
            var data = p.Data;

            string idU = data.ID;
            string roleGuid = data.Role;
            bool delete = data.Operation;

            Guid userids = idU == null ? Guid.Parse("00000000-0000-0000-0000-000000000000") : Guid.Parse(idU);

            var repository = factory.GetRepository<Repository<Roles>>();
            var query = repository.GetQueryable();
            var oRole = query.Where(o => o.Id == userids).FirstOrDefault();


            if (!delete)
            {
                if (oRole == null)
                {
                    oRole = new Roles()
                    {
                        Name = data.Name,
                        Description = data.Description

                    };
                }
                else
                {
                    oRole.Name = data.Name;
                    oRole.Description = data.Description;

                }


                factory.OnTransaction(() =>
                {
                    oRole = repository.SaveOrUpdate(oRole);
                });
            }
            else
            {
                //DeletePrivileges(oRole);
                factory.OnTransaction(() =>
                {
                    var oli = new List<Privileges>(oRole.RolePrivileges);
                    foreach(var ol in oli)
                    {
                        oRole.RolePrivileges.Remove(ol);
                    }
                    
                    repository.Delete(oRole);
                });
            }

            View.AddControl(ControlPath.IAccountsRolesView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }

        private Roles GetRoleById(Guid guid)
        {

            var repository = factory.GetRepository<Repository<Roles>>();
            var query = repository.GetQueryable();
            return query.Where(o => o.Id == guid).FirstOrDefault();

        }

        private void PageSettingsChanged()
        {
            string _virtualPath = View.PostParameter.VirtualPath;
            string _pagerContext = View.PostParameter.PagerContext;
            switch (_pagerContext)
            {
                case "accounts.users.list":
                    View.AddControl(ControlPath.IAccountsUsersView, View.ControlPlaceHolder);
                    break;
                case "accounts.roles":
                    View.AddControl(ControlPath.IAccountsRolesView, View.ControlPlaceHolder);
                    break;
            }

            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }

        private void ProcessEditGrid()
        {
            View.AddControl(ControlPath.IAccountsAddEditView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }
        private void ProcessRolesGrid()
        {
            View.AddControl(ControlPath.IAccountsManageView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }


        private void ProcessShowTab()
        {
            ClearPageSettings();
            string datatarget = View.PostParameter.DataTarget;
            switch (datatarget)
            {
                case "users.tab":
                    View.AddControl(ControlPath.IAccountsUsersView, View.ControlPlaceHolder);
                    break;
                case "roles.tab":
                    View.AddControl(ControlPath.IAccountsRolesView, View.ControlPlaceHolder);
                    break;
                case "privileges.tab":
                    View.AddControl(ControlPath.IAccountsPrivilegesView, View.ControlPlaceHolder);
                    break;
                case "user.add":
                    View.AddControl(ControlPath.IAccountsAddEditView, View.ControlPlaceHolder);
                    break;
                case "user.view":
                    View.AddControl(ControlPath.IAccountsUsersView, View.ControlPlaceHolder);
                    break;
                case "roles.add":
                    View.AddControl(ControlPath.IAccountsManageView, View.ControlPlaceHolder);
                    break;
                case "roles.view":
                    View.AddControl(ControlPath.IAccountsRolesView, View.ControlPlaceHolder);
                    break;
            }




            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }


    }

    public interface IAccountsView : IBasePage, IView
    {
        CallbackPanel ControlPanel { get; set; }
        PlaceHolder ControlPlaceHolder { get; set; }
    }



}