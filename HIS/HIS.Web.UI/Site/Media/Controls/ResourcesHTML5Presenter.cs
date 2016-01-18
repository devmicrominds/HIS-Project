using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class ResourcesHTML5Presenter : GenericPresenter<IResourcesHTML5View> {

        private IRepositoryFactory factory;
        private string page_key = "resources.html5";

        public ResourcesHTML5Presenter(IRepositoryFactory factory) 
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

            View.JsonData = Json(new
            {
                TypeSelectionFont = Fonts(),
                SelectedFont = "",
                TypeSelectionFontSize = FontSize(),
                SelectedFontSize = ""
            });
        }

        private PagedResult<ResourceTitle> GetViewModel() 
        {        
            var repository = factory.GetRepository<Repository<ResourceTitle>>();
            var query = repository.GetQueryable();

            query = query.Where(o => o.ResourceType == (int)ResourceType.HTML);

            return repository.PagedResult(query, PageSettingsList[page_key].PageIndex, PageSettingsList[page_key].PageSize);
        }

        private dynamic Fonts()
        {
            var repository = factory.GetRepository<Repository<Fonts>>();
            var query = repository.GetQueryable();

            var list = new List<object>();
            list.AddRange(query.Select(o => new { title = o.FontName, value = o.Id }));
            return list.ToArray();
        }

        private dynamic FontSize()
        {
            var repository = factory.GetRepository<Repository<FontSize>>();
            var query = repository.GetQueryable();

            var list = new List<object>();
            list.AddRange(query.Select(o => new { title = o.Size, value = o.Id }));
            return list.ToArray();
        }
    }
}