<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlayerEdit.ascx.cs" Inherits="HIS.Web.UI.Site.Terminal.Controls.PlayerEdit" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">
            <div class="mailbox-content tabbable tab-right">
                <ul id="tablist" class="nav nav-pills">
                     <li><a href="#player_view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
                    <li class="active"><a href="#player_add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
                </ul>
            </div>
            <div class="tab-content">
                <div id="tab_view" class="tab-pane fade active in">
                    <div class="body">
                        <div id="Div4" class="form-horizontal">
                            <fieldset>
                              
                                <legend style="padding-left: 10px;">Add/Edit Player</legend>
                                <div class="control-group">
                                    <label class="control-label" for="name">Name</label>
                                    <div class="controls form-group">
                                        <div class="col-sm-8">
                                            <input id="name" class="form-control" placeholder="Name"
                                                required="required" type="text" maxlength="25" data-validation="required" />
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="location">Location</label>
                                    <div class="controls form-group">
                                        <div class="col-sm-6">
                                            <input id="location" class="form-control" placeholder="Location"
                                                required="required" type="text" maxlength="25" data-validation="required" />
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="group_role">Group</label>
                                    <div class="controls form-group">
                                        <div id="group_select_placeholder" class="col-sm-8">
                                            <select id="group_role" data-style="btn-primary" data-validation="required"></select>
                                        </div>
                                    </div>
                                </div>



                            </fieldset>
                            <div class="form-actions">

                                <button  data-action="edit_User" type="button" class="btn btn-success">Save </button>
                            
                                <button data-action="reset_User" type="button" class="btn btn-danger">Reset</button>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </section>
    </div>
</div>
<script type="text/javascript">
    __JsonModel(<%= JsonData %>);
    var SelectionData = __jsonModel;


    var PlayerEditView = Backbone.View.extend(
    {

        initialize: function (options) {
            var self = this;
            self.options = options;
            $("#name").val(self.options.model.Name);
            $("#location").val(self.options.model.Location);

            var snippet = _.template($('#role_select_template').html(),
            {
                content: self.model.TypeSelection,
                selectedgroup: self.model.SelectedGroup
            });

            $('#group_role').append(snippet);
            $('#group_role').selectpicker();
            $.validate();

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
        events:
        {
            'click button[data-action=edit_User]': '_editPlayer',
            'click button[data-action=reset_User]': '_resetPlayer',
        },
        _editPlayer: function (e) {
            var self = this;
            var oJson = {};
            oJson.Action = 'EditPlayer';
            oJson.DateTimeRequest = moment().format();
            oJson.Data =
            {
                Operation: 'false',
                Name: $('#name').val(),
                Location: $('#location').val(),
                GroupID: $('#group_role').val(),
                PlayerID: SelectionData.PlayerID,

            };

            if (self._checking())
                _Post(document.URL, oJson);
            else
                alert("Please Fill Empty Form");
        },
        _resetPlayer: function (e) {
            var self = this;
            $('#name').val('');
            $('#location').val('');
            console.log(self.model.TypeSelection);
            console.log(self.model.SelectedGroup);

            var snippet = _.template($('#role_select_template').html(),
            {
                content: self.model.TypeSelection,
                selectedgroup: ''
            });
            $('#group_role').empty();
            $('#group_role').append(snippet);

        },
        _checking: function (e) {
            if (!$('#name').val())
                return false;
            if (!$('#location').val())
                return false;
            if (!$('#group_role').val())
                return false;

            return true;

        },
    });




    $(function () {

        __JsonModel(<%= this.JsonData %>);
        console.log(__jsonModel);
        var view = new PlayerEditView(
          {
              model: __jsonModel,
              el: '#tab_view'
          });



    });





</script>

<script id="role_select_template" type="text/html">
    <@  _.each(content,function(o,e)  { console.log("**************") ;console.log(o.value); @>  
        <option value="<@= o.value @>" <@ if(selectedgroup==o.value) { @> selected="selected" <@  } @> ><@= o.title @></option>
    <@ });@> 
</script>


