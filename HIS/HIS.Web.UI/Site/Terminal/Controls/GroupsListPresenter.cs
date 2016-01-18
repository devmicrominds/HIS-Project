using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class GroupsListPresenter   : GenericPresenter<IGroupsListView>
    {
        const string page_key = "groups.list";

        private IRepositoryFactory factory;

        public GroupsListPresenter(IRepositoryFactory factory) {

            this.factory = factory;
        }

        public override void View_Load(object sender, EventArgs e)
        {
            if (!PageSettingsList.ContainsKey("groups.list"))
                PageSettingsList["groups.list"] = PageSettings.Default();

            var model = GetPagedGroups();
            ;
           
            View.LocalPageSettings = PageSettingsList[ page_key];
            View.Grid.VirtualItemCount = model.ItemCount;
            View.Grid.DataSource = model.Items;
            View.Grid.DataBind();
        }

        private PagedResult<Groups> GetPagedGroups()
        {
            var repository = factory.GetRepository<Repository<Groups>>();
            var query = repository.GetQueryable();

            return repository.PagedResult(query, PageSettingsList[page_key].PageIndex,
                                                    PageSettingsList[page_key].PageSize);
             
        }
    }
}