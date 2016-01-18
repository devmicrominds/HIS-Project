<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Resources_Stream.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_Stream" %>
<div id="resource_stream_view">
    <div class="header">
        <h4>Stream Contents</h4>
    </div>
    <div id="tab_view">
        <asp:GridView ID="__grid" ItemType="HIS.Data.ResourceStream" CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
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
                <asp:TemplateField HeaderText="Description" ItemStyle-Width="15%">
                    <ItemTemplate>
                        <%# Item.Description %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Path" ItemStyle-Width="25%">
                    <ItemTemplate>
                        <%# Item.Location %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Login" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <%# Item.Login %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Password" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <%# Item.Password %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <sym:Pager ID="Pager1" runat="server" PagerContext="resources.stream" />
            </PagerTemplate>
            <EmptyDataTemplate>
                There are currently no items in this table.
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</div>
<script type="text/javascript">

    $(function ()
    {
        console.log('Do Much!');

    });
</script>