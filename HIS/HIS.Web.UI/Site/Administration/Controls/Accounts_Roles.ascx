<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Accounts_Roles.ascx.cs" Inherits="HIS.Web.UI.Site.Administration.controls.Accounts_Roles" %>
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
                <li class="active"><a class="" href="#roles.tab" data-toggle="tab">
                    <em class="lead">Roles</em>
                </a></li>
                <li class=""><a class="" href="#privileges.tab" data-toggle="tab">
                    <em class="lead">Privileges</em>
                </a></li>
                <li class="divider"></li>
            </ul>
            <div class="mailbox-content tabbable">
                <ul id="accounts_user-menu" class="nav nav-pills">
                    <li class="active"><a href="#roles.view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
                    <li><a href="#roles.add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
                </ul>
                <div class="tab-content">
                    <div id="tab_view" class="tab-pane fade active in">
                        <asp:GridView ID="uGrid" ItemType="HIS.Data.Roles"
                            CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true"
                            AlternatingRowStyle-BackColor="White" AllowPaging="true" AllowCustomPaging="true"
                            ShowFooter="true" runat="server">
                            <PagerSettings Position="Top" Visible="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="#" ItemStyle-Width="15px">
                                    <ItemTemplate>
                                        <%# GridViewHelper.GetGridViewRowIndex(this.uGrid,Container.DataItemIndex) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <%# Item.Name %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <%# Item.Description %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Edit">
                                    <ItemTemplate>
                                        <button data-uid="<%# Item.Id %>" type="button" data-action="edit_Roles" class="editplayer btn btn-sm btn-info"><i class="fa fa-laptop"></i></button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Delete">
                                    <ItemTemplate>
                                        <button data-uid="<%# Item.Id %>" type="button" data-action="delete_Roles" class="editplayer btn btn-sm btn-info"><i class="fa fa-laptop"></i></button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <sym:pager id="Pager1" runat="server" pagercontext="accounts.roles" />
                            </PagerTemplate>
                            <EmptyDataTemplate>
                                There are currently no items in this table.
                            </EmptyDataTemplate>
                        </asp:GridView>

                    </div>


                </div>
            </div>
        </section>
    </div>
</div>
<script type="text/javascript">

    var AccountRolesView = Backbone.View.extend(
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
            'click button[data-action=edit_Roles]': '_editRoles',
            'click button[data-action=delete_Roles]': '_deleteRoles',

        },
          _editRoles: function (e) {

              var cmd = $(e.target);
              var datatarget = cmd.data('uid');
              console.log(datatarget);
              var oJson = {};
              oJson.Action = 'EditRolesGrid';
              oJson.DateTimeRequest = moment().format();
              oJson.DataTarget = datatarget;
              _Post(document.URL, oJson);
          },
          _deleteRoles: function (e) {

              var cmd = $(e.target);
              var datatarget = cmd.data('uid');
              var r = confirm("Sure Want To Delete!");
              if (r) {
                  var oJson = {};
                  oJson.Action = 'SaveRoles';
                  oJson.DateTimeRequest = moment().format();
                  oJson.Data =
                  {
                      Operation: 'true',
                      Name: '',
                      Description: '',
                      ID: datatarget,

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

        var view = new AccountRolesView(
          {
              model: __jsonModel,
              el: '#tab_view'
          });





    });




</script>

