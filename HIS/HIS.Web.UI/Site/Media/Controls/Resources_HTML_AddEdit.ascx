<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Resources_HTML_AddEdit.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_HTML_AddEdit" %>
<style>
    .align-controls {
        padding: 5px;
    }
</style>
<div class="row">
    <div class="col-md-12">

        <div class="tab-content">
            <div id="tab_view" class="tab-pane fade active in">
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
                                <input id="txtMainTitle" class="form-control" placeholder="Main Title"
                                    required="required" type="text" maxlength="50" data-validation="required" />
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top" style="white-space: nowrap; padding-top: 10px;">
                                <label class="control-label">URL</label>
                            </td>
                            <td class="align-controls" valign="top">
                                <input id="txtName" class="form-control" placeholder="Name"
                                    required="required" type="text" maxlength="50" data-validation="required" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">Font
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
                                    <button type="button" class="btn btn-success" data-action="add_Html5">Save</button>
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

    var ResourcesHtmlAddEdit = Backbone.View.extend({

        initialize: function () {
            var self = this;
            self.forecolor = '#3289c7';
            self.backcolor = '#3289c7';
            self.UpdateId = "";

            var snippetfont = _.template($('#font_select_template').html(),
            {
                content: self.model.TypeSelectionFont,
                selectedfont: self.model.SelectedFont
            });
            console.log(snippetfont);
            $('#font').append(snippetfont);
            $('#font').selectpicker();

            var snippetfontsize = _.template($('#fontsize_select_template').html(),
            {
                content: self.model.TypeSelectionFontSize,
                selectedfontsize: self.model.SelectedFontSize
            });
            console.log(snippetfontsize);
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

        _render: function () {
            var self = this;
        },

        events:
            {
                'click button[data-action=add_Html5]': '_add_Html5',
            },

        _add_Html5: function (e) {
            var self = this;
            self._saveData();
        },

        _saveData: function () {
            var self = this;
            console.log(self);
            if (self._validatedata()) {
                amplify.request(Backbone.REQUEST.ADDEDIT_HTML,
                    JSON.stringify(
                {                    
                    Name: $("#txtMainTitle").val(),
                    MainTitle: $("#txtMainTitle").val(),
                    ResourceType: 5,
                    ForeColor: self.forecolor,
                    Backcolor: self.backcolor,
                    Font: $("#font").val(),
                    FontSize: $("#fontsize").val()
                }))
            }
        },

        _validatedata: function () {
            if (!$('#txtMainTitle').val()) {
                alert('Please Enter Main Title');
                return false;
            }
            if (!$('#txtName').val()) {
                alert('Please Enter URL');
                return false;
            }
            return true;
        },

    });

    $(document).ready(function () {

        __JsonModel(<%= this.JsonData %>);

        var resourcesHtmlAddEdit = new ResourcesHtmlAddEdit({
            el: "#tab_view",
            model: __jsonModel,
        });
        resourcesHtmlAddEdit._render();
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

