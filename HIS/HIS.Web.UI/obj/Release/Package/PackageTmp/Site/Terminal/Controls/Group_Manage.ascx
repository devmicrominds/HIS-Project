<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Group_Manage.ascx.cs" Inherits="HIS.Web.UI.Site.Terminal.Controls.Group_Manage" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">
            <div class="mailbox-content tabbable tab-right">
                <ul id="tablist" class="nav nav-pills">
                    <li><a href="#group.view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
                    <li class="active"><a href="#group.add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
                </ul>
            </div>
            <div class="tab-content">
                <div id="tab_view" class="tab-pane fade active in">
                    <div class="body">
                        <div id="Div4" class="form-horizontal">
                            <fieldset>

                                <legend style="padding-left: 10px;">Add/Edit Groups</legend>
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
                                    <label class="control-label" for="location">Description</label>
                                    <div class="controls form-group">
                                        <div class="col-sm-6">
                                            <input id="description" class="form-control" placeholder="Description"
                                                required="required" type="text" maxlength="25" data-validation="required" />
                                        </div>
                                    </div>
                                </div>


                            </fieldset>
                            <div class="form-actions">

                                <button data-action="edit_group" type="button" class="btn btn-success">Save </button>

                                <button data-action="reset_group" type="button" class="btn btn-danger">Reset</button>
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
    var SelectionGroupsData = __jsonModel;


    var GroupEditView = Backbone.View.extend(
    {

        initialize: function (options) {
            var self = this;
            self.options = options;
            $("#name").val(self.options.model.Name);
            $("#description").val(self.options.model.Description);



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
            'click button[data-action=edit_group]': '_editgroup',
            'click button[data-action=reset_group]': '_resetgroup',
        },
        _editgroup: function (e) {
            var self = this;
            var oJson = {};
            oJson.Action = 'SaveGroup';
            oJson.DateTimeRequest = moment().format();
            oJson.Data =
            {
                Operation: 'false',
                Name: $('#name').val(),
                Description: $('#description').val(),
                ID: SelectionGroupsData.RolesID,

            };

            if (self._checking())
                _Post(document.URL, oJson);
            else
                alert("Please Fill Empty Form");
        },
        _resetgroup: function (e) {
            var self = this;
            $('#name').val('');
            $('#description').val('');


        },
        _checking: function (e) {
            if (!$('#name').val())
                return false;
            if (!$('#description').val())
                return false;

            return true;

        },
    });




    $(function () {

        __JsonModel(<%= this.JsonData %>);
        console.log(__jsonModel);
        var view = new GroupEditView(
          {
              model: __jsonModel,
              el: '#tab_view'
          });



    });





</script>
