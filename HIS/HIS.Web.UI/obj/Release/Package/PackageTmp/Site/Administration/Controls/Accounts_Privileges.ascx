<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="Accounts_Privileges.ascx.cs" Inherits="HIS.Web.UI.Site.Administration.controls.Accounts_Privileges" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">
            <ul id="accounts-menu" class="nav nav-pills pull-right">
                <li class="divider">
                    <br />
                </li>
                <li class=""><a class="" href="#users.tab" data-toggle="tab">
                    <em class="lead">Users</em>
                </a></li>
                <li class=""><a class="" href="#roles.tab" data-toggle="tab">
                    <em class="lead">Roles</em>
                </a></li>
                <li class="active"><a class="" href="#privileges.tab" data-toggle="tab">
                    <em class="lead">Privileges</em>
                </a></li>
                <li class="divider"></li>
            </ul>
            <div class="mailbox-content tabbable">
                <ul id="accounts_user-menu" class="nav nav-pills">
                    <li class="active"><a href="#tab_view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
                    <li><a href="#tab_add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
                </ul>
                <div class="tab-content">
                    <div id="tab_view" class="tab-pane fade active in">
                        <div class="body">
                            <div id="user_form" class="form-horizontal">
                                <fieldset>
                                    <legend style="padding-left: 10px;"> Privilege Selection </legend>
                                </fieldset>
                                 
                                <div class="control-group">

                                    <label class="control-label" for="user_role">Role</label>
                                    <div class="controls form-group">
                                        <div id="role_select_placeholder" class="col-sm-8">
                                            <select id="user_role" data-style="btn-primary" data-validation="required"></select>
                                        </div>
                                    </div>
                                </div>


                                <div class="form-actions">
                                    <button id="BtnFind" type="button" class="btn btn-success">Find</button>
                                </div>



                            </div>
                            <asp:GridView ID="__grid" ItemType="HIS.Web.UI.Contents.model.RolesPrivilegeViewModel" CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" AlternatingRowStyle-BackColor="White" AllowPaging="true" AllowCustomPaging="true"
                                ShowFooter="true"
                                runat="server">
                                <PagerSettings Position="Top" Visible="true" />
                                <Columns>
                                    <asp:TemplateField HeaderText="#" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <%# GridViewHelper.GetGridViewRowIndex(this.__grid,Container.DataItemIndex) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Privilege Description">
                                        <ItemTemplate>
                                            <%# Item.PrivilegeDesc %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 <%--   <asp:TemplateField HeaderText="Accessibility">
                                        <ItemTemplate>
                                            <%# Item.Accessibility %>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                     <asp:TemplateField HeaderText="Accessibility">
                                        <ItemTemplate>
                                            <input data-id="<%# Item.PrivilegeId %>"  type="checkbox" class="accessibility" <%# (Item.Accessibility) ? "checked" : ""  %> />                        
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     
                                </Columns>
                                <PagerTemplate>
                                    <sym:pager id="Pager1" runat="server" pagercontext="accounts.privileges" />
                                </PagerTemplate>
                                <EmptyDataTemplate>
                                    There are currently no items in this table.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="tab_add" class="tab-pane fade in">
                </div>
            </div>
        </section>
    </div>
</div>

<script type="text/javascript">


    var AccountPrivileges =
   {
       initialize: function (options) {
           var self = this;
           this.options = options;


           $('#accounts-menu a[data-toggle="tab"]').on('show.bs.tab', function (e) {
               var datatarget = e.target.href.split('#')[1];
               console.log(datatarget);
               var oJson = {};
               oJson.Action = 'ShowTab';
               oJson.DateTimeRequest = moment().format();
               oJson.DataTarget = datatarget;
               _Post(document.URL, oJson);

           });


           $('#BtnFind').on('click', function (e) {

               var role = $("#user_role").val();
               if (role == "") {
                   alert("Please Select Role");
               }
               else {

                   var oJson = {};
                   oJson.Action = 'FindRoles';
                   oJson.DateTimeRequest = moment().format();
                   oJson.Data =
                   {
                       Role: $('#user_role').val(),
                   };
                   _Post(document.URL, oJson);
               }

           });



           var snippet = _.template($('#role_select_template').html(),
           {
               content: self.options.Roles,
               selectedrole: self.options.SelectedRole
           });

           $('#user_role').append(snippet);
           $('#user_role').selectpicker();
           $.validate();



           var els = $('.accessibility');
           _.each(els, function (o, e) {
               new Switchery(o);
           });

           els.on('change', function (e) {

               var elx = $(e.target);

               var oJson = {};
               oJson.Action = 'UpdatePrivilege';
               oJson.DateTimeRequest = moment().format();
               oJson.Data =
               {
                   Role: $('#user_role').val(),
                   PrivillageID: elx.data('id'),
               };
               _Post(document.URL, oJson);


           });



       },


   };




    $(function () {

        __JsonModel(<%= this.JsonData %>);

        console.log(__jsonModel);
        AccountPrivileges.initialize(__jsonModel);

        /*var elem = document.querySelector('.demo');
        var init = new Switchery(elem, {
            color: '#fec200',
            secondaryColor: '#41b7f1'
        });*/



    });



</script>
<script id="role_select_template" type="text/html">
    <@  _.each(content,function(o,e)  { @>  
        <option value="<@= o.value @>" <@ if(selectedrole==o.value) { @> selected="selected" <@  } @> ><@= o.title @></option>
    <@ });@> 
</script>

