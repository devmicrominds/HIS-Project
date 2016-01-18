using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HIS.Web.UI 
{
    public class AccountsRolesPresenter : GenericPresenter<IAccountsRolesView>
    {
        private IRepositoryFactory factory;
        const string  roles_pagekey = "accounts.roles";
        
        public AccountsRolesPresenter(IRepositoryFactory factory) {

            this.factory = factory;
           
             
        }

        public override void ViewBinded() 
        {
            
            if (!PageSettingsList.ContainsKey(roles_pagekey))
                PageSettingsList[roles_pagekey] = PageSettings.Default();
            this.View.Load += View_Load;
        }

        public override void View_Load(object sender, EventArgs e)
        {
            View.Model = GetPagedRoles();
            BindGridView();
        }

        private void BindGridView()
        {
            View.LocalPageSettings = this.PageSettingsList[roles_pagekey];
            View.MainGrid.VirtualItemCount = View.Model.ItemCount;
            View.MainGrid.DataSource = View.Model.Items;
            View.MainGrid.DataBind();

        }

        private PagedResult<Roles> GetPagedRoles()
        {
            var repository = factory.GetRepository<Repository<Roles>>();
            var query = repository.GetQueryable();
            // filter here
            return repository.PagedResult(query, PageSettingsList[roles_pagekey].PageIndex,
                PageSettingsList[roles_pagekey].PageSize);

        }

    }

  
}