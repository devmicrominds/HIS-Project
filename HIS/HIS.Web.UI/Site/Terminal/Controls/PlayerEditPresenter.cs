using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.Terminal.Controls
{

    public class PlayerEditPresenter : GenericPresenter<IPlayerEditView>
    {
        private IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;
        public PlayerEditPresenter(IRepositoryFactory factory)
        {
            this.factory = factory;
            BuildRunActions();
        }

        private void BuildRunActions()
        {
            RunAction = new Dictionary<string, Action>() 
            {
                 
                 { "EditPlayerGrid",EditGrid },
                 { "ShowTab",ProcessShowTab },
                 
            };
        }
        public void ProcessShowTab()
        {
            var p = View.PostParameter;
            var data = p.Data;
            View.JsonData = Json(new
            {
                TypeSelection = Group(),
                SelectedGroup = ""
            });
        }

        public void EditGrid()
        {
            var p = View.PostParameter;
            var data = p.Data;
            string guidString = p.DataTarget;
            var player = getPlayer(new Guid(guidString));
            Guid test = player.Groups.Id;
            View.JsonData = Json(new
            {
                TypeSelection = Group(),
                PlayerID = guidString,
                Name = player.Name,
                Location = player.Location,
                SelectedGroup = player.Groups.Id
            });


        }

        private dynamic getPlayer(Guid guid)
        {

            var repository = factory.GetRepository<Repository<Player>>();

            var query = repository.GetQueryable();

            return query.Where(o => o.Id == guid)
                               .Single();

        }

        private dynamic Group()
        {

            var repository = factory.GetRepository<Repository<HIS.Data.Groups>>();

            var query = repository.GetQueryable();

            var list = new List<object>();

            list.Add(new { title = @"Group  ", value = "" });

            list.AddRange(query.Select(o => new { title = o.Name, value = o.Id }));

            return list.ToArray();

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
    }
}