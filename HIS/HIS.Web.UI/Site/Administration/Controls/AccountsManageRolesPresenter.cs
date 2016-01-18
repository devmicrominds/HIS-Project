using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.Administration.Controls
{



    public class AccountsManageRolesPresenter : GenericPresenter<IAccountsManageRolesView>
    {
        IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;
        public AccountsManageRolesPresenter(IRepositoryFactory factory)
        {

            this.factory = factory;
            BuildRunActions();
        }
        public override void View_Load(object sender, EventArgs e)
        {
            var parameter = View.PostParameter;
            if (parameter != null)
            {
                string action = parameter.Action;

                if (RunAction.ContainsKey(action))
                {
                    RunAction[action]();
                }
            }

        }

        private void BuildRunActions()
        {
            RunAction = new Dictionary<string, Action>() 
            {
                 
                 { "EditRolesGrid",ProcessEditGrid },
                 { "ShowTab",ProcessShowTab },
                 
            };
        }
        public void ProcessShowTab()
        {
            var p = View.PostParameter;
            var data = p.Data;
            View.JsonData = Json(new
            {
                /*TypeSelection = Roles(),*/
                SelectedRoles = ""
            });

        }




        public void ProcessEditGrid()
        {
            var p = View.PostParameter;
            var data = p.Data;
            string guidString = p.DataTarget;
            var _role = GetSingleRoles(new Guid(guidString));
            View.JsonData = Json(new
            {
                RolesID = guidString,
                Name = _role.Name,
                Description = _role.Description,
            });
        }
        public dynamic GetSingleRoles(Guid id)
        {
            var repository = factory.GetRepository<Repository<Roles>>();
            var query = repository.GetQueryable();
            return query.Where(x => x.Id == id).FirstOrDefault();
        }

    }
}