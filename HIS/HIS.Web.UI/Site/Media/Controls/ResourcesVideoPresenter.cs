using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HIS.Web.UI
{
    public class ResourcesVideoPresenter : GenericPresenter<IResourcesVideoView>
    {
        private IRepositoryFactory factory;
        private string page_key = "resources.videocategory";

        public ResourcesVideoPresenter(IRepositoryFactory factory)
        {
            this.factory = factory;
        }

        public override void View_Load(object sender, EventArgs e)
        {

            if (!PageSettingsList.ContainsKey(page_key)) PageSettingsList[page_key] = PageSettings.Default();



            View.Model = GetViewModel();
            View.LocalPageSettings = PageSettingsList[page_key];
            View.Grid.VirtualItemCount = View.Model.ItemCount;
            View.Grid.DataSource = View.Model.Items;
            View.Grid.DataBind();

        }

        private PagedResult<MediaCategory> GetViewModel()
        {
            var repository = factory.GetRepository<Repository<MediaCategory>>();
            var query = repository.GetQueryable();
            //query = query.Where(x => x.MediaResourceType == ResourceType.Video);

            var query2 = query.Where(o => o.MediaResourceType == ResourceType.Video);

            return repository.PagedResult(query2, PageSettingsList[page_key].PageIndex, PageSettingsList[page_key].PageSize);
        }
    }
}