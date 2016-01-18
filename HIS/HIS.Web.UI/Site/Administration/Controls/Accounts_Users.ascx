<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Accounts_Users.ascx.cs" Inherits="HIS.Web.UI.Site.Administration.controls.Accounts_Users" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">
            <ul id="accounts-menu" class="nav nav-pills pull-right">
                <li class="divider">
                    <br />
                </li>
                <li class="active"><a class="" href="#users.tab" data-toggle="tab">
                    <em class="lead">Users</em>
                </a></li>
                <li class=""><a class="" href="#roles.tab" data-toggle="tab">
                    <em class="lead">Roles</em>
                </a></li>
                <li class=""><a class="" href="#privileges.tab" data-toggle="tab">
                    <em class="lead">Privileges</em>
                </a></li>
                <li class="divider"></li>
            </ul>
            <div class="mailbox-content tabbable tab-right">
                <ul id="accounts_user-menu" class="nav nav-pills">
                    <li class="active"><a href="#user.view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
                    <li><a href="#user.add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
                </ul>
            </div>
            <div class="tab-content">
                <div id="tab_view" class="tab-pane fade active in">
                    <asp:GridView ID="__grid" ItemType="HIS.Data.Users" CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" AlternatingRowStyle-BackColor="White" AllowPaging="true" AllowCustomPaging="true"
                        ShowFooter="true" runat="server">
                        <PagerSettings Position="Top" Visible="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="#" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <%# GridViewHelper.GetGridViewRowIndex(this.__grid,Container.DataItemIndex) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <%# Item.UserName %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <%# Item.Email %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Role">
                                <ItemTemplate>
                                    <%# Item.UserRoles.Name %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="15%" HeaderText="Edit">
                                <ItemTemplate>
                                    <button data-uid="<%# Item.Id %>" type="button" data-action="edit_User" class="editplayer btn btn-sm btn-info"><i class="fa fa-laptop"></i></button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="15%" HeaderText="Delete">
                                <ItemTemplate>
                                    <button data-uid="<%# Item.Id %>" type="button" data-action="delete_User" class="editplayer btn btn-sm btn-info"><i class="fa fa-laptop"></i></button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <sym:pager id="Pager1" runat="server" pagercontext="accounts.users" />
                        </PagerTemplate>
                        <EmptyDataTemplate>
                            There are currently no items in this table.
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>

            </div>
        </section>
    </div>
</div>

<script type="text/javascript">

    var AccountUserView = Backbone.View.extend(
      {

          initialize: function (options) {
              var self = this;
              self.options = options;


              $('#accounts-menu a[data-toggle="tab"]').on('show.bs.tab', function (e) {

                  self._showTab(e);
              });

              $('#accounts_user-menu a[data-toggle="tab"]').on('show.bs.tab', function (e) {
                  self._showTab(e);
              });

          },
          events:
        {
            'click button[data-action=edit_User]': '_editUser',
            'click button[data-action=delete_User]': '_deleteUser',

        },
          _editUser: function (e) {

              var cmd = $(e.target);
              var datatarget = cmd.data('uid');
              var oJson = {};
              oJson.Action = 'EditUserGrid';
              oJson.DateTimeRequest = moment().format();
              oJson.DataTarget = datatarget;
              _Post(document.URL, oJson);
          },
          _deleteUser: function (e) {

              var cmd = $(e.target);
              var datatarget = cmd.data('uid');
              var r = confirm("Sure Want To Delete!");
              if (r) {
                  var oJson = {};
                  oJson.Action = 'SaveUser';
                  oJson.DateTimeRequest = moment().format();
                  oJson.Data =
                  {
                      ID: datatarget,
                      Operation: 'true',
                      Username: '',
                      Password: '',
                      Email: '',
                      Role: '',


                  };
                  _Post(document.URL, oJson);
              }
          },
          _showTab: function (e) {
              var datatarget = e.target.href.split('#')[1];
              var oJson = {};
              oJson.Action = 'ShowTab';
              oJson.DateTimeRequest = moment().format();
              oJson.DataTarget = datatarget;
              _Post(document.URL, oJson);
          },
      });

    $(function () {

        __JsonModel(<%= this.JsonData %>);

        console.log(__jsonModel);

        var view = new AccountUserView(
          {
              model: __jsonModel,
              el: '#tab_view'
          });





    });




</script>













