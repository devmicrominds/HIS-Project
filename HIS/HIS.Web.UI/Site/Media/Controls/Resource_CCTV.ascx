<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Resource_CCTV.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resource_CCTV" %>
<style>
    .align-controls {
        padding: 5px;
    }
</style>
<div id="resource_cctv_view">
    
    <div id="resources_image_category_menu" class="mailbox-content tab-right">
        <ul class="nav nav-pills">
            <li class="active"><a href="#tab_view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
            <li><a href="#tab_add" data-toggle="tab" data-action="add_newrecord"><i class="glyphicon glyphicon-plus"></i></a></li>
        </ul>
    </div>
    <div class="tab-content">
        <div id="tab_view" class="tab-pane fade in active">
            <div class="header">
        <h4>CCTV Data</h4>
    </div>
            <asp:GridView ID="__grid" ItemType="HIS.Data.ResourceTitle" CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
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
                    <asp:TemplateField HeaderText="Title" ItemStyle-Width="25%">
                        <ItemTemplate>
                            <%# Item.Title %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IP" ItemStyle-Width="30%">
                        <ItemTemplate>
                            <%# Item.Name %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="15%" HeaderText="Edit">
                        <ItemTemplate>
                            <button data-uid="<%# Item.Id %>" type="button" data-action="edit_cctvdata" class="editplayer btn btn-sm btn-info"><i class="fa fa-edit"></i></button>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="15%" HeaderText="Delete">
                        <ItemTemplate>
                            <button data-uid="<%# Item.Id %>" type="button" data-action="delete_cctvdata" class="editplayer btn btn-sm btn-info"><i class="glyphicon glyphicon-remove-sign"></i></button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerTemplate>
                    <sym:Pager ID="Pager1" runat="server" PagerContext="resources.cctv" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    There are currently no items in this table.
                </EmptyDataTemplate>
            </asp:GridView>
        </div>

        <div id="tab_add" class="tab-pane fade">
            <div class="body">
                <fieldset>
                    <legend style="padding-left: 10px;">CCTV Data Management</legend>
                </fieldset>

                <div class="col-sm-12" style="padding-top:30px;">
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td valign="top" style="white-space: nowrap; padding-top: 10px;">
                                <label class="control-label">Title</label>
                            </td>
                            <td class="align-controls" valign="top">
                                <input id="txtMainTitle" class="form-control" placeholder="Title" style="width:450px;"
                                    required="required" type="text" maxlength="50" data-validation="required" />
                            </td>

                        </tr>
                        <tr>
                            <td valign="top" style="white-space: nowrap; padding-top: 10px;">
                                <label class="control-label">IP</label>
                            </td>
                            <td class="align-controls" valign="top">
                                <input id="txtName" class="form-control" placeholder="IP Address"  style="width:450px;"
                                    required="required" type="text" maxlength="15" data-validation="required" />
                            </td>
                        </tr>
                        <tr >
                            <td colspan="2" style="padding-top:30px;">
                                <div class="form-actions" style="padding-left: 0px; text-align: center;">
                                    <button type="button" class="btn btn-success" data-action="save_CCTVData">Save</button>
                                </div>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">

    var ResourceCCTVView = Backbone.View.extend
    ({
        initialize: function () {
            var self = this;
            self._$menu = this.$el.find('#resources_image_category_menu');
            self.recordId = null;
            $.validate();
        },

        _render: function () {
            var self = this;
        },

        events:
       {
           'click button[data-action=edit_cctvdata]': '_editCCTVData',
           'click button[data-action=delete_cctvdata]': '_deleteCCTVData',
           'click button[data-action=save_CCTVData]': '_saveCCTVData',
           'click a[data-action=add_newrecord]': '_addnewrecord',
       },

        _addnewrecord: function () {
            var self = this;
            self._resetPage();
        },

        _editCCTVData: function (e) {
            var self = this;
            var cmd = $(e.target);
            var datatarget = cmd.data('uid');
            self.recordId = datatarget;
            self._getData(datatarget);
            $('#tab_view').removeClass('in active');
            $('#tab_add').addClass('in active');
        },

        _getData: function (datatarget) {
            var self = this;
            $.ajax({
                url: '/app/Media/GetCCTVData',
                type: 'GET',
                datatype: 'json',
                data: { id: datatarget },
                success: function (data) {
                    console.log(data);
                    $('#txtMainTitle').val(data.resource.Title);
                    Name: $("#txtName").val(data.resource.Name);
                }
            });
        },

        _saveCCTVData: function (e) {
            var self = this;
            console.log(self);
            self._saveData();
        },

        _saveData: function () {
            var self = this;
            console.log(self);
            if (self._validatedata()) {
                var data = {
                    Action: "SaveCCTVData",
                    Data: {
                        Context: 'Stream',
                        Name: $("#txtName").val(),
                        MainTitle: $("#txtMainTitle").val(),
                        ResourceType: 7,
                        ID: self.recordId,
                        Operation: 0/*Add or Edit*/
                    }
                };
                amplify.publish('RESOURCE_MENU_ACTION_REQUEST', data);
                self._resetPage();
            }
        },

        _deleteCCTVData: function (e) {
            var cmd = $(e.target);
            var datatarget = cmd.data('uid');
            var r = confirm("Are you sure you want To delete the record");
            if (r) {
                var data = {
                    Action: "SaveCCTVData",
                    Data: {
                        Context: 'Stream',
                        ID: datatarget,
                        Operation: 1/*Delete*/
                    }
                };
                amplify.publish('RESOURCE_MENU_ACTION_REQUEST', data);
            }
        },

        _resetPage: function () {
            self.recordId = null;
            $('#txtMainTitle').val('');
            $('#txtName').val('');
        },

        _validatedata: function () {
            var self = this;
            if (!$('#txtMainTitle').val()) {
                alert('Please Enter Title');
                return false;
            }
            if (!$('#txtName').val()) {
                alert('Please Enter IP');
                return false;
            }

            if (!self._validateIPaddress($('#txtName').val())) {
                alert('You have entered an invalid IP address');
                return false;
            }
            return true;
        },

        _validateIPaddress: function (e) {
            if (/^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/.test(e)) {
                return true;
            }
            else
                return false;
        },
    });

    $(function () {
        var resCCTV = new ResourceCCTVView(
        {
            el: '#resource_cctv_view',
        });
        resCCTV.render();
    });
</script>
