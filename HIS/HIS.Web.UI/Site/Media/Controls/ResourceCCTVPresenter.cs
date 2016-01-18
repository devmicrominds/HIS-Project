using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.Media.Controls
{
    public class ResourceCCTVPresenter : GenericPresenter<IResourceCCTVView>
    {
        private IRepositoryFactory factory;
        private string page_key = "resources.cctv";

        public ResourceCCTVPresenter(IRepositoryFactory factory) 
        {            
            this.factory = factory;
        }

        public override void View_Load(object sender, EventArgs e)
        {
            if (!PageSettingsList.ContainsKey(page_key))
                PageSettingsList[page_key] = PageSettings.Default();

            View.Model = GetViewModel();
            View.LocalPageSettings = PageSettingsList[page_key];
            View.Grid.VirtualItemCount = View.Model.ItemCount;
            View.Grid.DataSource = View.Model.Items;
            View.Grid.DataBind();
        }

        private PagedResult<ResourceTitle> GetViewModel()
        {
            var repository = factory.GetRepository<Repository<ResourceTitle>>();
            var query = repository.GetQueryable();

            query = query.Where(o => o.ResourceType == (int)ResourceType.CCTV);

            return repository.PagedResult(query, PageSettingsList[page_key].PageIndex, PageSettingsList[page_key].PageSize);
        }
    }
}