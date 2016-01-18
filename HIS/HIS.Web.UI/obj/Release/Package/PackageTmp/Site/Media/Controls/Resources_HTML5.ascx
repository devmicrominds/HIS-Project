<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Resources_HTML5.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_HTML5" %>
<div id="resource_html5_view">
    <div class="header">
        <h4>HTML5 Contents</h4>
    </div>
    <div id="tab_view">
        <asp:GridView ID="__grid" ItemType="HIS.Data.ResourceHTML" CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
            ShowHeader="true" AlternatingRowStyle-BackColor="White" AllowPaging="true" AllowCustomPaging="true" ShowHeaderWhenEmpty="false"
            ShowFooter="true"
            runat="server">
            <PagerSettings Position="Top" Visible="true" />
            <Columns>
                <asp:TemplateField HeaderText="#" ItemStyle-Width="1%">
                    <ItemTemplate>
                        <%# GridViewHelper.GetGridViewRowIndex(this.__grid,Container.DataItemIndex) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <%# Item.Name %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" ItemStyle-Width="55%">
                    <ItemTemplate>
                        <%# Item.Description %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <sym:Pager ID="Pager1" runat="server" PagerContext="resources.html5" />
            </PagerTemplate>
            <EmptyDataTemplate>
                There are currently no items in this table.
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</div>

<script type="text/javascript">

    var ResourceHTMLView = Backbone.View.extend
    ({
        initialize: function (options) {
            var self = this;
            self.options = options;
            console.log(options);
           
        },
        events: { 'click button[data-toggle="action"]': 'actionRequest' },
        actionRequest: function (e) {
            var self = this;
            var element = $(e.target);
            var target = element.data('target');
            var uuid = element.data('uid');

            var data = {
                Action: target,
                Data: {
                    Context: 'Image',
                    UID: uuid,
                    Name: $('#name').val(),
                    Description: $('#description').val(),
                    Resource: '2',
                    Color: self.color
                }
            };
            amplify.publish('RESOURCE_MENU_ACTION_REQUEST', data);
        },
        render: function ()
        {


        }

    });

    $(function ()
    {
        __JsonModel(<%= this.JsonData %>);

        var resHTML = new ResourceHTMLView(
        {
            el: '#resource_html5_view',
            model: __jsonModel, oiu: ''
        });
        resHTML.render();

    });



</script>



