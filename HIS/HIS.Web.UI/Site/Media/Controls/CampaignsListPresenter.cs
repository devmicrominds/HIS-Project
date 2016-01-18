using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class CampaignsListPresenter : GenericPresenter<ICampaignsListView>
    {
        private string page_key = "media.campaigns";
        private IRepositoryFactory factory;

        public CampaignsListPresenter(IRepositoryFactory factory) {
            
            this.factory = factory;
        }

        public override void View_Load(object sender, EventArgs e)
        {
            if (!PageSettingsList.ContainsKey(page_key)) PageSettingsList[ page_key] = PageSettings.Default();

            View.Model = GetViewModel();
            View.LocalPageSettings = PageSettingsList[page_key];
            View.Grid.VirtualItemCount = View.Model.ItemCount;
            View.Grid.DataSource = View.Model.Items;
            View.Grid.DataBind();

             
        }

        public PagedResult<Campaigns> GetViewModel() 
        {

            var repository = factory.GetRepository<Repository<Campaigns>>();

            var query = repository.GetQueryable();

            return repository.PagedResult(query, PageSettingsList[page_key].PageIndex,
                                          PageSettingsList[page_key].PageSize);
        }

        
    }
}