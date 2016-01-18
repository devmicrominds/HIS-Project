using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class ResourcesMusicPresenter : GenericPresenter<IResourcesMusicView>
    {
        private IRepositoryFactory factory;
        private string page_key = "resources.musiccategory";

        public ResourcesMusicPresenter(IRepositoryFactory factory)
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

        private PagedResult<MediaCategory> GetViewModel()
        {
            var repository = factory.GetRepository<Repository<MediaCategory>>();
            var query = repository.GetQueryable();

            //IQueryable<MediaCategory> query2 = query.Where(o => o.MediaResourceType == ResourceType.Music);
            var query2 = query.Where(o => o.MediaResourceType == ResourceType.Music);
            return repository.PagedResult(query2, PageSettingsList[page_key].PageIndex, PageSettingsList[page_key].PageSize);
        }
    }
}