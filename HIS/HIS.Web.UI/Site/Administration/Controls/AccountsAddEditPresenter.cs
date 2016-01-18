using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.Administration.Controls
{
    public class AccountsAddEditPresenter : GenericPresenter<IAccountsAddEditView>
    {
        IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;
        public AccountsAddEditPresenter(IRepositoryFactory factory)
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
                 
                 { "EditUserGrid",ProcessEditGrid },
                 { "ShowTab",ProcessShowTab },
                 
            };
        }
        public void ProcessShowTab()
        {
            var p = View.PostParameter;
            var data = p.Data;
            View.JsonData = Json(new
            {
                TypeSelection = Roles(),
                SelectedRoles = ""
            });

        }




        public void ProcessEditGrid()
        {
            var p = View.PostParameter;
            var data = p.Data;
            string guidString = p.DataTarget;
            var _user = GetSingleUser(new Guid(guidString));
            View.JsonData = Json(new
            {
                USERID = guidString,
                TypeSelection = Roles(),
                Name = _user.UserName,
                Email = _user.Email,
                Password = _user.UserPassword,
                SelectedRoles = _user.UserRoles.Id
            });
        }
        public dynamic GetSingleUser(Guid id)
        {
            var repository = factory.GetRepository<Repository<Users>>();
            var query = repository.GetQueryable();
            return query.Where(x => x.Id == id).FirstOrDefault();
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