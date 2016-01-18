using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class PlayersListPresenter : GenericPresenter<IPlayersListView>
    {
        const string page_key = "players.list";

        private IRepositoryFactory factory;

        public PlayersListPresenter(IRepositoryFactory factory)
        {

            this.factory = factory;

        }



        public override void View_Load(object sender, EventArgs e)
        {

            if (!PageSettingsList.ContainsKey("players.list"))
                PageSettingsList["players.list"] = PageSettings.Default();
            //View.JsonData = Json(Group());
            BindGridView();

        }



        private void BindGridView()
        {

            var model = GetPagedGroups();

            View.LocalPageSettings = PageSettingsList[page_key];
            View.Grid.VirtualItemCount = model.ItemCount;
            View.Grid.DataSource = model.Items;
            View.Grid.DataBind();
        }

        private PagedResult<Player> GetPagedGroups()
        {
            var repository = factory.GetRepository<Repository<Player>>();
            var query = repository.GetQueryable();

            return repository.PagedResult(query, PageSettingsList[page_key].PageIndex,
                                                    PageSettingsList[page_key].PageSize);

        }

        //Get Group Name

        //private dynamic Group()
        //{

        //    var repository = factory.GetRepository<Repository<Groups>>();

        //    var query = repository.GetQueryable();

        //    var list = new List<object>();

        //    list.Add(new { title = @"Group  ", value = "" });

        //    list.AddRange(query.Select(o => new { title = o.Name, value = o.Id }));

        //    return list.ToArray();

        //}
    }
}