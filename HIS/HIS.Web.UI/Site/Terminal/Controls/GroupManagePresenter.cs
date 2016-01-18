using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.Terminal.Controls
{
    public class GroupManagePresenter : GenericPresenter<IGroupManageView>
    {
        IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;
        public GroupManagePresenter(IRepositoryFactory factory)
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

                { "EditGroupGrid",ProcessEditGrid },
                //{ "ShowTab",ProcessShowTab },

            };
        }

        public void ProcessEditGrid()
        {
            var p = View.PostParameter;
            var data = p.Data;
            string guidString = p.DataTarget;
            var _group = GetSingleRoles(new Guid(guidString));
            View.JsonData = Json(new
            {
                RolesID = guidString,
                Name = _group.Name,
                Description = _group.Description,
            });
        }
        public dynamic GetSingleRoles(Guid id)
        {
            var repository = factory.GetRepository<Repository<HIS.Data.Groups>>();
            var query = repository.GetQueryable();
            return query.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}

