using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI
{
    public class AccountsUsersPresenter : GenericPresenter<IAccountsUsersView>
    {
        IRepositoryFactory factory;
        const string userlist_page_key = "accounts.users.list";
        private IDictionary<string, Action> RunAction;
        public AccountsUsersPresenter(IRepositoryFactory factory)
        {

            this.factory = factory;

        }



        public override void View_Load(object sender, EventArgs e)
        {

            if (!PageSettingsList.ContainsKey(userlist_page_key))
                PageSettingsList[userlist_page_key] = PageSettings.Default();

            View.Model = GetPagedUsers();
            //View.JsonData = Json(Roles());
            BindGridView();

        }

        private void BindGridView()
        {
            View.LocalPageSettings = PageSettingsList[userlist_page_key];
            View.MainGrid.VirtualItemCount = View.Model.ItemCount;
            View.MainGrid.DataSource = View.Model.Items;
            View.MainGrid.DataBind();

        }



        private PagedResult<Users> GetPagedUsers()
        {
            var repository = factory.GetRepository<Repository<Users>>();
            var query = repository.GetQueryable();
            // filter here

            //var L2EQuery = from st in query
            //               where st.UserName == "Hasan"
            //               select st;

            return repository.PagedResult(query, PageSettingsList[userlist_page_key].PageIndex, PageSettingsList[userlist_page_key].PageSize);

        }

        private dynamic UserFormsSchema()
        {

            dynamic result = new
            {
                schema = GetUserSchema(),
                form = new object[] {
                new
                {
                    type = "fieldset",
                    title = "New User",
                    required = true,
                    items = new object[]
                    {
                        new { 
                            key="username", type="text", placeholder="Name", 
                            dataAttr= new 
                            { 
                                validation="required"
                            } 
                        },
                        new { 
                            key="userpassword", type="text", placeholder="Password" ,
                            dataAttr=new 
                            {
                                validation="required"                            
                            }
                        },
                         new {
                            key="email", type="text", placeholder="Email",
                            dataAttr=new
                            {
                                validation="required"
                            }
                        },
                        new {
                            key="role", type="select",
                            fieldHtmlClass="selectpicker", 
                            dataAttr= new 
                            {
                                style="btn-primary",
                                validation="required",
                            },
                            items="",//Roles(),
                        },
                       
                        

                    }
                 },
                 new {
                    
                     type="actions",
                     items=new object[] 
                     {
                        new {  id="cmdsave", type="button", title="Save", htmlClass="btn-primary" },
                        new { id="cmdreset", type="button", title="Reset", htmlClass="btn-danger"  },
                     }

                 
                 }   
              }



            };

            return result;
        }

        //private dynamic Roles()
        //{

        //    var repository = factory.GetRepository<Repository<Roles>>();

        //    var query = repository.GetQueryable();

        //    var list = new List<object>();

        //    list.Add(new { title = @"Role  ", value = "" });

        //    list.AddRange(query.Select(o => new { title = o.Name, value = o.Id }));

        //    return list.ToArray();

        //}

        private dynamic GetUserSchema()
        {

            return new
            {
                username = new
                {
                    required = true,
                    type = "string",
                    title = "Name"
                },
                userpassword = new
                {
                    required = true,
                    type = "string",
                    title = "Password"
                },
                email = new
                {
                    required = true,
                    type = "string",
                    title = "Email"
                },
                role = new
                {
                    required = true,
                    type = "array",
                    title = "Roles"
                }
            };
        }
    }



}