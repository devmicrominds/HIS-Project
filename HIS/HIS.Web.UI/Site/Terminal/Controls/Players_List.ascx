<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Players_List.ascx.cs" Inherits="HIS.Web.UI.Site.Terminal.Controls.Players_List" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">
            <div class="mailbox-content tabbable tab-right">
                <ul id="tablist" class="nav nav-pills">
                    <li class="active"><a href="#player_view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
                    <li><a href="#player_add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
                </ul>
            </div>
            <div class="tab-content">
                <div id="tab_view" class="tab-pane fade active in">
                    <asp:GridView ID="__grid" ItemType="HIS.Data.Player" CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
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
                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <%# Item.Location %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Group">
                                <ItemTemplate>
                                    <%# Item.Groups.Name %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="15%" HeaderText="Edit">
                                <ItemTemplate>
                                    <button data-uid="<%# Item.Id %>" type="button" class="editplayer btn btn-sm btn-info"><i class="fa fa-laptop"></i></button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="15%" HeaderText="Delete">
                                <ItemTemplate>
                                    <button data-uid="<%# Item.Id %>" type="button" class="deleteplayer btn btn-sm btn-info"><i class="fa fa-laptop"></i></button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <sym:pager id="Pager1" runat="server" pagercontext="players.list" />
                        </PagerTemplate>
                        <EmptyDataTemplate>
                            There are currently no items in this table.
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                <div id="tab_add" class="tab-pane fade in">
                </div>
            </div>
        </section>
    </div>
</div>
<script type="text/javascript">


    var PlayerList =
    {
        initialize: function (options) {
            var self = this;
            this.options = options;


            $('.editplayer').on('click', function (e) {
                var cmd = $(e.target);
                var datatarget = cmd.data('uid');
                var oJson = {};
                oJson.Action = 'EditPlayerGrid';
                oJson.DateTimeRequest = moment().format();
                oJson.DataTarget = datatarget;
                _Post(document.URL, oJson);




            });

            $('.deleteplayer').on('click', function (e) {
                var cmd = $(e.target);
                var datatarget = cmd.data('uid');
                console.log(datatarget);
                var r = confirm("Sure Want To Delete!");
                if (r) {
                    var oJson = {};
                    oJson.Action = 'EditPlayer';
                    oJson.Data =
                {
                    Operation: 'true',
                    Name: '',
                    Location: '',
                    GroupID: '',
                    PlayerID: datatarget,

                };


                    _Post(document.URL, oJson);
                }

            });

            $('#tablist a[data-toggle="tab"]').on('show.bs.tab', function (e) {
                var datatarget = e.target.href.split('#')[1];
                console.log(datatarget);
                var oJson = {};
                oJson.Action = 'ShowTab';
                oJson.DateTimeRequest = moment().format();
                oJson.DataTarget = datatarget;
                _Post(document.URL, oJson);

            });

        },



    };


    $(function () {

        __JsonModel(<%= this.JsonData %>);
        console.log(__jsonModel);
        PlayerList.initialize(__jsonModel);

    });

</script>


