<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Resource_Ticker.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resource_Ticker" %>
    <style>
        .align-controls
        {
            padding: 5px;
        }
    </style>
    <div class="row">
        <div class="col-md-12">

            <div class="tab-content">
                <div id="tab_view" class="tab-pane fade active in">
                    <fieldset>
                        <legend style="padding-left: 10px;">Ticker Management</legend>
                    </fieldset>

                    <div class="col-sm-12">
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td valign="top" style="white-space: nowrap; padding-top:10px;">
                                    <label class="control-label">Title</label>
                                </td>
                                <td class="align-controls">
                                    <input id="txtMainTitle" class="form-control" placeholder="Main Title"
                                        required="required" type="text" maxlength="50" data-validation="required" />
                                </td>
                                <td>&nbsp;</td>
                                 <td valign="top" style="white-space: nowrap; padding-top:10px;">
                                     <label class="control-label">Name</label>
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
                                <td valign="top" style="white-space: nowrap; padding-top:10px;">
                                    <label class="control-label">Ticker Text</label>
                                </td>
                                <td colspan="3" class="align-controls">
                                    <input id="ticker" class="form-control" placeholder="Ticker Text"
                                        required="required" type="text" maxlength="300" data-validation="required" />
                                </td>
                                <td class="align-controls" valign="top">
                                    <button type="button" id="add" class="btn btn-info" data-action="add_Content">Add</button>
                                    <button type="button" id="update" data-action="update_Content" class="btn btn-info">Update</button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <div id="listView">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <div class="form-actions" style="padding-left: 0px; text-align: center;">
                                        <button type="button" class="btn btn-success" data-action="add_Ticker">Save</button>
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

    var ResourceTicker = Backbone.View.extend({

        initialize: function () {
            var self = this;
            self.m_model = [];
            self.m_test = "";
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

            self.model = [];

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
            /*self._getData();*/
            $("#update").hide();
        },

        _getData: function () {
            /*var self = this;
            $.ajax({
                url: '/app/Media/GetTickers',
                type: 'GET',
                datatype: 'json',
                success: function (data) {
                    alert(data);
                    _.each(data, function () {
                        console.log(data);
                    });
                }

            });*/
        },

        events:
            {
                'click button[data-action=delete_Content]': '_delete_Content',
                'click button[data-action=add_Content]': '_add_Content',
                'click button[data-action=filter_up]': '_filterUp',
                'click button[data-action=filter_down]': '_filterDown',
                'click button[data-action=add_Ticker]': '_add_Ticker',
                'click button[data-action=edit_Content]': '_edit_Content',
                'click button[data-action=update_Content]': '_update_Content',
            },

        _filterDown: function (e) {
            console.log($(e.target));
            var self = this;
            var cmd = $(e.target);
            var datatarget = cmd.data('uid');
            console.log(cmd.data('uid'));
            data = self.model;
            var effID = datatarget + 1;
            self._sortingData(data, effID, datatarget)
        },

        _filterUp: function (e) {
            var self = this;
            var cmd = $(e.target);
            var datatarget = cmd.data('uid');
            data = self.model;
            var effID = datatarget - 1;
            self._sortingData(data, effID, datatarget)
        },

        _sortingData: function (data, effID, currID) {
            var self = this;
            self.m_model = [];
            _.each(data, function (properties, block) {
                if (properties.Id == currID) {
                    self.m_model[block] = { Id: effID, Title: properties.Title, OrderBy: effID };
                }
                else {
                    if (properties.Id == effID) {
                        self.m_model[block] = { Id: currID, Title: properties.Title, OrderBy: currID };
                    }
                    else {
                        self.m_model[block] = properties;
                    }
                }
            });
            self._sortingGrid(self.m_model);
        },

        _sortingGrid: function (data) {
            var self = this;
            self.m_model = [];
            _.each(data, function (properties, block) {
                var id = properties.Id - 1;
                self.m_model[id] = properties;
            });
            self._bindToGrid(self.m_model);
        },

        _delete_Content: function (e) {
            var self = this;
            var cmd = $(e.target);
            var datatarget = cmd.data('uid');
            self._deleted(datatarget)
        },

        _edit_Content: function (e) {
            $("#add").hide();
            $("#update").show();
            var self = this;
            var data = self.model;
            var cmd = $(e.target);
            var datatarget = cmd.data('uid');
            self._Edit(datatarget, data)
        },

        _Edit: function (currID, data) {
            var self = this;
            self.UpdateId = currID;
            self.m_model = [];
            _.each(data, function (properties, block) {
                console.log(properties);
                if (properties.Id == currID) {
                    $("#ticker").val(properties.Title);
                }
            });
        },

        _update_Content: function () {
            var self = this;
            var data = self.model;
            console.log(self.model);
            _.each(data, function (properties, block) {
                if (properties.Id == self.UpdateId) {
                    self.model[block] = { Id: self.UpdateId, Title: $("#ticker").val(), OrderBy: block };
                    console.log(self.model);
                    self._bindToGrid(self.model);
                    $("#ticker").val('');
                    self.UpdateId = "";
                    $("#update").hide();
                    $("#add").show();
                }
            });
        },

        _add_Content: function (e) {
            var self = this;
            if (self._tickercontentcheck()) {
                var data = self.model;
                console.log(data);
                var max = Object.keys(data).length;
                /*_.each(data, function (properties, block) {                
                    max = block;                
                });*/
                console.log(Object.keys(data).length);
                var positon = parseInt(max, 10) + 1;
                var index = parseInt(max, 10) + 1;
                data[positon] =
                {
                    Id: index,
                    Title: $("#ticker").val(),
                    OrderBy: positon
                };
                console.log(data);
                console.log('position =' + positon + "index =" + index);
                self.model = data;
                self._sortingGrid(self.model);
                $("#ticker").val('');
            }
        },

        _add_Ticker: function (e) {
            var self = this;
            self._saveData();
        },

        _saveData: function () {
            var self = this;
            var data = self.model;
            console.log(data);
            if (self._tickerdatacheck(data)) {
                amplify.request(Backbone.REQUEST.ADD_TICKER,
                    JSON.stringify(
                {
                    Tickers: data,
                    Name: $("#txtMainTitle").val(),
                    MainTitle: $("#txtMainTitle").val(),
                    ResourceType:4,
                    ForeColor: self.forecolor,
                    Backcolor: self.backcolor,
                    Font: $("#font").val(),
                    FontSize: $("#fontsize").val()
                }))
            }
        },

        _deleted: function (e) {
            var self = this;
            data = self.model;

            self.m_model = [];
            var cnn = 0;
            _.each(data, function (properties, block) {
                if (properties.Id != e) {
                    properties.OrderBy = properties.Id = cnn + 1;
                    self.m_model[cnn] = properties;
                    cnn = cnn + 1;
                }
            });
            self._bindToGrid(self.m_model);
        },

        _bindToGrid: function (data) {
            var self = this;
            console.log(data);
            console.log(self.model);
            self.model = data;
            console.log(self.model);
            var snippet = _.template($('#tickers_template').html(),
                {
                    resources: data,
                    datalength: Object.keys(data).length,
                });
            $('#listView').empty();
            if(Object.keys(data).length>0)
                $('#listView').append(snippet);
        },

        _tickercontentcheck: function (e) {
            if (!$('#ticker').val()) {
                alert('Please Enter Ticker Text');
                return false;
            }
            return true;
        },

        _tickerdatacheck: function (e) {                       
            if (!$('#txtMainTitle').val()) {
                alert('Please Enter Main Title');
                return false;
            }
            if (!$('#txtName').val()) {
                alert('Please Enter Name');
                return false;
            }            
            if (!(Object.keys(e).length > 0)) {
                alert('Please Add Ticker Text');
                return false;
            }
            return true;
        },

    });

    $(document).ready(function () {

        __JsonModel(<%= this.JsonData %>);

        var resourceTicker = new ResourceTicker({
            el: "#tab_view",
            model: __jsonModel,
        });
        resourceTicker._render();


    });
</script>

<script id="tickers_template" type="text/template">
<div class="row">
        <table class="table table-striped table-images">
            <thead>
                <tr>    
                    <th class="hidden-xs-portrait">Order</th>
                    <th>Title</th>
                    <th>Move Up</th>
                    <th>Move Down</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <@ _.each(resources,function(o,e){  @>
               
                <tr>                    
                    <td><@= o.Id @></td>
                    <td><@= o.Title @></td>
                    <td>
                          <@if (o.Id != 1) { @>
                                <button data-uid="<@= o.Id @>" type="button" data-action="filter_up" class="btn btn-small btn-info"><i class="glyphicon glyphicon-arrow-up"></i></button>
                          <@}@>
                        </td>

                         <td>
                           <@
                               
                                if(o.Id != datalength)
                                  {
                            @>
                          <button data-uid="<@= o.Id @>" type="button" data-action="filter_down" class="btn btn-small btn-info"><i class="glyphicon glyphicon-arrow-down"></i></button>
                            <@
                                 }
                             @>
                        </td>
                    <td> <button data-uid="<@= o.Id @>" type="button" data-action="edit_Content" class="editcampaign btn btn-small btn-info"><i class="fa fa-edit"></i></button> </td>
                    <td> <button data-uid="<@= o.Id @>" type="button" data-action="delete_Content" class="editplayer btn btn-sm btn-info"><i class="glyphicon glyphicon-remove-sign"></i></button> </td>
                    
                </tr>
                <@ }); @>
            </tbody>
        </table>
    </div>
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

