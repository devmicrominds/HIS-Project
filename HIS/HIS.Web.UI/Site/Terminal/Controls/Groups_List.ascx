<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Groups_List.ascx.cs" Inherits="HIS.Web.UI.Site.Terminal.Controls.Groups_List" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">
            <div class="mailbox-content tabbable tab-right">
                <ul id="groups_list-menu" class="nav nav-pills">
                    <li class="active"><a href="#group.view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
                    <li><a href="#group.add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
                </ul>
            </div>
            <div class="tab-content">
                <div id="tab_view" class="tab-pane fade active in">
                    <asp:GridView ID="__grid" ItemType="HIS.Data.Groups" CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
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
                            <asp:TemplateField ItemStyle-Width="15%" HeaderText="Schedule">
                                <ItemTemplate>
                                    <button data-uid="<%# Item.Id %>" type="button" data-action="edit_schedule" class="edit_schedule btn btn-sm btn-info"><i class="fa fa-laptop"></i></button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="15%" HeaderText="Edit">
                                <ItemTemplate>
                                    <button data-uid="<%# Item.Id %>" type="button" data-action="edit_grid" class="edit_grid btn btn-sm btn-info"><i class="fa fa-laptop"></i></button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <sym:pager id="Pager1" runat="server" pagercontext="groups.list" />
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

    var GroupListView = Backbone.View.extend(
      {

          initialize: function (options) {
              var self = this;
              self.options = options;


              $('#groups_list-menu a[data-toggle="tab"]').on('show.bs.tab', function (e) {
                  self._showTab(e);
              });


          },
          events:
        {
            'click button[data-action=edit_schedule]': '_editschedule',
            'click button[data-action=edit_grid]': '_editgrid',

        },
          _editschedule: function (e) {
              var cmd = $(e.target);
              var datatarget = cmd.data('uid');
              var self = this;
              self._postEvent('EditSchedule', datatarget, moment().format());
          },
          _editgrid: function (e) {
              var cmd = $(e.target);
              var datatarget = cmd.data('uid');
              var self = this;
              self._postEvent('EditGroupGrid', datatarget, moment().format());



          },
          _showTab: function (e) {
              var datatarget = e.target.href.split('#')[1];
              var self = this;
              self._postEvent('ShowTab', datatarget, moment().format());
          },
          _postEvent: function () {
              var oJson = {};
              oJson.Action = arguments[0];
              oJson.DateTimeRequest = arguments[2];
              oJson.DataTarget = arguments[1];

              _Post(document.URL, oJson);

          },
      });




    $(function () {

        __JsonModel(<%= this.JsonData %>);

        console.log(__jsonModel);

        var view = new GroupListView(
          {
              model: __jsonModel,
              el: '#tab_view'
          });





    });




</script>

