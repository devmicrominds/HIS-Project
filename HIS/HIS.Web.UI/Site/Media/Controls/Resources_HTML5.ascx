<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Resources_HTML5.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_HTML5" %>
<style>
    .align-controls {
        padding: 5px;
    }
</style>
<div id="resource_html5_view">

    <div id="resources_image_category_menu" class="mailbox-content tab-right">
        <ul class="nav nav-pills">
            <li class="active"><a href="#tab_view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
            <li><a href="#tab_add" data-toggle="tab" data-action="add_newrecord"><i class="glyphicon glyphicon-plus"></i></a></li>
        </ul>
    </div>
    <div class="tab-content">
        <div id="tab_view" class="tab-pane fade in active">
            <div class="header">
                <h4>HTML5 Contents</h4>
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
                    <asp:TemplateField HeaderText="URL" ItemStyle-Width="40%">
                        <ItemTemplate>
                            <%# Item.Name %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="15%" HeaderText="Edit">
                        <ItemTemplate>
                            <button data-uid="<%# Item.Id %>" type="button" data-action="edit_htmldata" class="editplayer btn btn-sm btn-info"><i class="fa fa-edit"></i></button>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="15%" HeaderText="Delete">
                        <ItemTemplate>
                            <button data-uid="<%# Item.Id %>" type="button" data-action="delete_htmldata" class="editplayer btn btn-sm btn-info"><i class="glyphicon glyphicon-remove-sign"></i></button>
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

        <div id="tab_add" class="tab-pane fade">
            <div class="body">
                <fieldset>
                    <legend style="padding-left: 10px;">HTML5 Management</legend>
                </fieldset>

                <div class="col-sm-12">
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td valign="top" style="white-space: nowrap; padding-top: 10px;">
                                <label class="control-label">Title</label>
                            </td>
                            <td class="align-controls" valign="top">
                                <input id="txtMainTitle" class="form-control" placeholder="Title"
                                    required="required" type="text" maxlength="50" data-validation="required" />
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top" style="white-space: nowrap; padding-top: 10px;">
                                <label class="control-label">URL</label>
                            </td>
                            <td class="align-controls" valign="top">
                                <input id="txtName" class="form-control" placeholder="URL"
                                    required="required" type="text" maxlength="200" data-validation="required" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle"><label class="control-label" for="font">Font</label>
                            </td>
                            <td class="align-controls" valign="top">
                                <div id="font_select_placeholder" class="col-sm-8" style="padding-left: 0px;">
                                    <select id="font" data-style="btn-primary" data-validation="required">
                                    </select>
                                </div>
                            </td>
                            
                            <td>&nbsp;</td>
                            <td valign="middle" style="white-space: nowrap;">Font Size
                            </td>
                            <td class="align-controls" valign="top">
                                <div id="font_size_placeholder" class="col-sm-8" style="padding-left: 0px;">
                                    <select id="fontsize" data-style="btn-primary" data-validation="required">
                                    </select>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="white-space: nowrap;">Fore Color
                            </td>
                            <td class="align-controls">
                                <div id="pickerForeColor"></div>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <label class="control-label" style="white-space: nowrap;">Back Color</label>
                            </td>
                            <td class="align-controls">
                                <div id="pickerBackColor"></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div class="form-actions" style="padding-left: 0px; text-align: center;">
                                    <button type="button" class="btn btn-success" data-action="save_htmldata">Save</button>
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

    var ResourceHTMLView = Backbone.View.extend
    ({
        initialize: function (options) {
            var self = this;
            self._$menu = this.$el.find('#resources_image_category_menu');
            self.recordId = null;            
            self.forecolor = '#1389c7';
            self.backcolor = '#3289c7';

            var snippetfont = _.template($('#font_select_template').html(),
            {
                content: self.model.TypeSelectionFont,
                selectedfont: self.model.SelectedFont
            });
            $('#font').append(snippetfont);
            $('#font').selectpicker();
            

            var snippetfontsize = _.template($('#fontsize_select_template').html(),
            {
                content: self.model.TypeSelectionFontSize,
                selectedfontsize: self.model.SelectedFontSize
            });
            $('#fontsize').append(snippetfontsize);
            $('#fontsize').selectpicker();
            $.validate();

            $('#pickerForeColor').colpick({
                flat: true,
                layout: 'hex',
                submit: 0,
                onChange: function (hsb, hex, rgb, el, bySetColor) {
                    self.forecolor = '#' + hex;
                },
            });

            $('#pickerBackColor').colpick({
                flat: true,
                layout: 'hex',
                submit: 0,
                onChange: function (hsb, hex, rgb, el, bySetColor) {
                    self.backcolor = '#' + hex;
                },
            });
        },

        events:
       {
           'click button[data-action=edit_htmldata]': '_editHtmlData',
           'click button[data-action=delete_htmldata]': '_deleteHtmlData',
           'click button[data-action=save_htmldata]': '_saveHtmlData',
           'click a[data-action=add_newrecord]': '_addnewrecord',
       },

        _addnewrecord: function ()
        {
            var self = this;
            self._resetPage();
        },

        _editHtmlData: function (e) {
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
                url: '/app/Media/GetHTMLData',
                type: 'GET',
                datatype: 'json',
                data: { id: datatarget },
                success: function (data) {
                    console.log(data);
                    $('#txtMainTitle').val(data.resource.Title);
                    $('#txtName').val(data.resource.Name);

                    $("#font").val(data.fontProperties.Font);
                    $('#font span.filter-option').text(data.fontProperties.Font);                    
                    $("#fontsize").val(data.fontProperties.FontSize);
                    $('#fontsize span.filter-option').text(data.fontProperties.FontSize);
                      
                    $('#pickerForeColor').colpickSetColor(data.fontProperties.ForeColor);
                    $('#pickerBackColor').colpickSetColor(data.fontProperties.BackgroundColor);
                    self.forecolor = data.fontProperties.ForeColor;
                    self.backcolor = data.fontProperties.BackgroundColor;                    
                }
            });
        },

        _saveHtmlData: function (e) {
            var self = this;
            self._saveData();
        },

        _saveData: function () {
            var self = this;
            console.log(self);
            if (self._validatedata()) {
                var data = {
                    Action: "SaveHTMLData",
                    Data: {
                        Context: 'HTML5',
                        Name: $("#txtMainTitle").val(),
                        MainTitle: $("#txtMainTitle").val(),
                        ResourceType: 5,
                        ForeColor: self.forecolor,
                        Backcolor: self.backcolor,
                        Font: $("#font").val(),
                        FontSize: $("#fontsize").val(),
                        ID: self.recordId,
                        Operation: 0/*Add or Edit*/
                    }
                };
                amplify.publish('RESOURCE_MENU_ACTION_REQUEST', data);
                self._resetPage();
            }
        },

        _deleteHtmlData: function (e) {
            var cmd = $(e.target);
            var datatarget = cmd.data('uid');
            var r = confirm("Are you sure you want To delete the record");
            if (r) {
                var data = {
                    Action: "SaveHTMLData",
                    Data: {
                        Context: 'HTML5',
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

            $("#font").val('Aharoni');
            $('#font span.filter-option').text('Aharoni');
            $("#fontsize").val('8');
            $('#fontsize span.filter-option').text('8');

            $('#pickerForeColor').colpickSetColor('#3289c7');
            $('#pickerBackColor').colpickSetColor('#3289c7');            
            self.forecolor = '#3289c7';
            self.backcolor = '#3289c7';
        },

        _validatedata: function () {
            if (!$('#txtMainTitle').val()) {
                alert('Please Enter Title');
                return false;
            }
            if (!$('#txtName').val()) {
                alert('Please Enter URL');
                return false;
            }
            return true;
        },

    });

    $(function () {
        __JsonModel(<%= this.JsonData %>);
        var resHTML = new ResourceHTMLView(
        {
            el: '#resource_html5_view',
            model: __jsonModel, oiu: ''
        });
        resHTML.render();
    });
</script>

<script id="font_select_template" type="text/html">
    <@  _.each(content,function(o,e)  { console.log("**************") ;console.log(o.value); @>  
        <option value="<@= o.title @>" <@ if(selectedfont==o.value) { @> selected="selected" <@  } @> ><@= o.title @></option>
    <@ });@> 
</script>

<script id="fontsize_select_template" type="text/html">
    <@  _.each(content,function(o,e)  { console.log("**************") ;console.log(o.value); @>  
        <option value="<@= o.title @>" <@ if(selectedfontsize==o.value) { @> selected="selected" <@  } @> ><@= o.title @></option>
    <@ });@> 
</script>



