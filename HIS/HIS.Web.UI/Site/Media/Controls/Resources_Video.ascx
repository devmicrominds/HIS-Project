<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Resources_Video.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_Video" %>
<div id="resource_video_view">
    
    <div id="resources_video_category_menu" class="mailbox-content tab-right">
        <ul class="nav nav-pills">
            <li class="active"><a href="#tab_view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
            <li><a href="#tab_add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
        </ul>
    </div>
    <div class="tab-content">
        <div id="tab_view" class="tab-pane fade in active">
            <div class="header">
        <h4>Video Categories</h4>
    </div>
            <asp:GridView ID="__grid" ItemType="HIS.Data.MediaCategory" CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
                ShowHeader="true" AlternatingRowStyle-BackColor="White" AllowPaging="true" AllowCustomPaging="true"
                ShowFooter="true"
                runat="server">
                <PagerSettings Position="Top" Visible="true" />
                <Columns>
                    <asp:TemplateField HeaderText="#" ItemStyle-Width="5%">
                        <ItemTemplate>
                            <%# GridViewHelper.GetGridViewRowIndex(this.__grid,Container.DataItemIndex) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name" ItemStyle-Width="25%">
                        <ItemTemplate>
                            <%# Item.Name %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" ItemStyle-Width="40%">
                        <ItemTemplate>
                            <%# Item.Description %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="# Tag" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <button type="button" class="editcolor btn btn-default" style='background: <%# Item.ColorCode %>; min-width: 25px; cursor:default;'>
                                &nbsp;
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <div class="btn-groups">
                                <button data-uid="<%# Item.Id %>" type="button" class="btn btn-small btn-info" data-toggle="action" data-target="ShowGallery">
                                    Show Videos
                                </button>
                                <button data-uid="<%# Item.Id %>" type="button" class="btn btn-small btn-info" data-toggle="action" data-target="FileUpload">
                                    Upload 
                                </button>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerTemplate>
                    <sym:Pager ID="Pager1" runat="server" PagerContext="resources.videocategory" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    There are currently no items in this table.
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
        <div id="tab_add" class="tab-pane fade">
            <div class="body">
                <div id="Div4" class="form-horizontal">
                    <fieldset>
                        <legend style="padding-left: 10px;">Add New Video Category</legend>                        
                        <div class="control-group" style="margin-bottom:-10px;">
                            <label class="control-label" for="name">Name</label>
                            <div class="controls form-group">
                                <div class="col-sm-6">
                                    <input id="name" class="form-control" placeholder="Category Name"
                                        required="required" type="text" maxlength="50" data-validation="required" />
                                </div>
                            </div>
                        </div>
                        <div class="control-group" style="margin-bottom:-10px;">
                            <label class="control-label" for="description">Description</label>
                            <div class="controls form-group">
                                <div class="col-sm-6">
                                    <input id="description" class="form-control" placeholder="Category Description"
                                        required="required" type="text" maxlength="150" data-validation="required" />
                                </div>
                            </div>
                        </div>
                        <div class="control-group" style="margin-bottom:-10px;">
                            <label class="control-label" for="name">Pick Color </label>
                            <div class="controls form-group">
                                <div class="col-sm-8">
                                    <div id="picker"></div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <div class="form-actions" style="padding-left: 0px; text-align: center;">
                        <button data-uid="saveUser>" type="button" class="btn btn-success" data-toggle="action" data-target="SaveMediaCategory">Save </button>
                        <button data-uid="resetUser>" type="button" class="btn btn-danger" data-action="reset_pagedata">Reset </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">

    var ResourceVideoView = Backbone.View.extend(
    {
        initialize: function (options) {
            var self = this;
            self.options = options;
            self._$menu = this.$el.find('#resources_video_category_menu');
            self.color = '#3289c7';
            $.validate();
            $('#picker').colpick({
                flat: true,
                layout: 'hex',
                submit: 0,
                onChange: function (hsb, hex, rgb, el, bySetColor) {
                    self.color = '#' + hex;

                },
            });
        },

        events: {
            'click button[data-toggle="action"]': 'actionRequest',
            'click button[data-action="reset_pagedata"]': '_resetpagedata'
        },

        actionRequest: function (e) {
            var self = this;
            var element = $(e.target);
            var target = element.data('target');
            var uuid = element.data('uid');

            var data = {
                Action: target,
                Data: {
                    Context: 'Video',
                    UID: uuid,
                    Name: $('#name').val(),
                    Description: $('#description').val(),
                    Resource: '3',
                    Color: self.color
                }
            };
            if (target == "SaveMediaCategory") {
                if (self._validatedata()) {
                    amplify.publish('RESOURCE_MENU_ACTION_REQUEST', data);
                }
            }
            else
                amplify.publish('RESOURCE_MENU_ACTION_REQUEST', data);
        },

        _resetpagedata: function () {
            $('#name').val('');
            $('#description').val('');
            self.color = '#3289c7';
            $('#picker').colpickSetColor('#3289c7');
        },

        _validatedata: function () {
            if (!$('#name').val()) {
                alert('Please Enter Video Category Name');
                return false;
            }
            if (!$('#description').val()) {
                alert('Please Enter Video Category Description');
                return false;
            }
            return true;
        },

        render: function () {

        }

    });

    $(function () {
        var resVideo = new ResourceVideoView({ el: '#resource_video_view' });
        resVideo.render();
    });

</script>
