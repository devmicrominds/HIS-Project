using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.Media.Controls
{
    public class ResourcesHTMLAddEditPresenter : GenericPresenter<IResourcesHTMLAddEditView>
    {
        private IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;

        public ResourcesHTMLAddEditPresenter(IRepositoryFactory factory)
        {
            this.factory = factory;
            //BuildRunActions();
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

        private void BuildRunActions()
        {
            RunAction = new Dictionary<string, Action>() 
            {                 
                 { "EditHTML",ProcessEditHTML },
            };
        }

        public void ProcessEditHTML()
        {
            var p = View.PostParameter;
            var data = p.Data;
            string guidString = p.DataTarget;
            //var _resource = GetSingleResource(new Guid(guidString));
            //var _fontProperties = GetResourceFontProperties(new Guid(guidString));
            View.JsonData = Json(new
            {
                //USERID = guidString,
                //TypeSelection = Roles(),
                //Name = _user.UserName,
                //Email = _user.Email,
                //Password = _user.UserPassword,
                //SelectedRoles = _user.UserRoles.Id
                UserId=""
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