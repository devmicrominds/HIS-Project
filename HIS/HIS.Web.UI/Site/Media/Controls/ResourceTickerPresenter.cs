using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HIS.Data;

namespace HIS.Web.UI.Site.Media.Controls
{
    public class ResourceTickerPresenter : GenericPresenter<IResourceTickerView>
    {
        IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;

        public ResourceTickerPresenter(IRepositoryFactory factory)
        {
            this.factory = factory;            
        }

        public override void View_Load(object sender, EventArgs e)
        {
            View.JsonData = Json(new
            {
                TypeSelectionFont = Fonts(),
                SelectedFont = "",
                TypeSelectionFontSize = FontSize(),
                SelectedFontSize = ""
            });
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