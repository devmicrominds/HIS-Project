<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Accounts_AddEdit.ascx.cs" Inherits="HIS.Web.UI.Site.Administration.Controls.Accounts_AddEdit" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">

            <div class="mailbox-content tabbable tab-right">
                <ul id="accounts_user-menu" class="nav nav-pills">
                    <li><a href="#user.view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
                    <li class="active"><a href="#user.add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
                </ul>
            </div>
            <div class="tab-content">
                <div id="tab_view" class="tab-pane fade active in">
                    <div class="body">
                        <div id="user_form" class="form-horizontal">
                            <fieldset>
                                <legend style="padding-left: 10px;">Add New User</legend>
                                <div class="control-group">
                                    <label class="control-label" for="user_name">Username</label>
                                    <div class="controls form-group">
                                        <div class="col-sm-8">
                                            <input id="user_name" class="form-control" placeholder="Username"
                                                required="required" type="text" maxlength="25" data-validation="required" />
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="user_password">Password</label>
                                    <div class="controls form-group">
                                        <div class="col-sm-6">
                                            <input id="user_password" class="form-control" placeholder="Password"
                                                required="required" type="password" maxlength="7" data-validation="required" />
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="user_confirm_password">Confirm Password</label>
                                    <div class="controls form-group">
                                        <div class="col-sm-6">
                                            <input id="user_confirm_password" class="form-control" placeholder="Confirm Password"
                                                required="required" type="password" maxlength="7" data-validation="required" />
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="user_email">Email</label>
                                    <div class="controls form-group">
                                        <div class="col-sm-8">
                                            <input id="user_email" class="form-control" type="email" placeholder="Email" maxlength="50"
                                                required="required" data-validation="email" data-validation-optional="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label" for="user_role">Role</label>
                                    <div class="controls form-group">
                                        <div id="role_select_placeholder" class="col-sm-8">
                                            <select id="user_role" data-style="btn-primary" data-validation="required"></select>
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
    var UserSelection = __jsonModel;


    var AccEditView = Backbone.View.extend(
    {

        initialize: function (options) {
            var self = this;
            self.options = options;

            $("#user_name").val(self.options.model.Name);
            $("#user_password").val(self.options.model.Password);
            $("#user_confirm_password").val(self.options.model.Password);
            $("#user_email").val(self.options.model.Email);

            var snippet = _.template($('#role_select_template').html(),
            {
                content: self.model.TypeSelection,
                selectedrole: self.model.SelectedRoles
            });

            $('#user_role').append(snippet);
            $('#user_role').selectpicker();
            $.validate();


            $('#accounts_user-menu a[data-toggle="tab"]').on('show.bs.tab', function (e) {
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
            'click button[data-action=edit_User]': '_saveUser',
            'click button[data-action=reset_User]': '_resetUser',
        },
        _saveUser: function (e) {
            var self = this;
            var oJson = {};
            oJson.Action = 'SaveUser';
            oJson.DateTimeRequest = moment().format();
            oJson.Data =
            {
                Operation: 'false',
                ID: UserSelection.USERID,
                Username: $('#user_name').val(),
                Password: $('#user_password').val(),
                Email: $('#user_email').val(),
                Role: $('#user_role').val(),


            };

            if (self._checking())
                _Post(document.URL, oJson);


            /*_Post(document.URL, oJson);*/

        },
        _resetUser: function (e) {
            var self = this;
            $('#user_name').val('');
            $('#user_password').val('');
            $('#user_confirm_password').val('');
            $('#user_email').val('');

            var snippet = _.template($('#role_select_template').html(),
           {
               content: self.model.TypeSelection,
               selectedrole: ''
           });
            $('#user_role').empty();
            $('#user_role').append(snippet);


        },
        _checking: function (e) {

            var self = this;
            if (!$('#user_name').val()) {
                alert('Please Enter User Name');
                return false;
            }
            if (!$('#user_password').val()) {
                alert('Please Enter Password');
                return false;
            }
            if (!$('#user_confirm_password').val()) {
                alert('Please Enter Confirmation Password');
                return false;
            }
            if (!$('#user_email').val()) {
                alert('Please Enter Email');
                return false;
            }
            if ($('#user_password').val().length < 7) {
                alert('Please Enter at least 7 character');
                return false;
            }

            if ($('#user_password').val() != $('#user_confirm_password').val()) {
                alert('Password Not Match');
                return false;
            }
            if (!self._emailValidation($('#user_email').val())) {
                alert('You have not given a correct e-mail address');
                return false;
            }
            if (!$('#user_role').val()) {
                alert('Please Select Role');
                return false;
            }


            return true;


        },
        _emailValidation: function (e) {
            console.log(e);
            var emailReg = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
            var valid = emailReg.test(e);
            console.log(valid);
            if (!valid) {
                return false;
            } else {
                return true;
            }

        },

    });




    $(function () {

        __JsonModel(<%= this.JsonData %>);
        console.log(__jsonModel);
        var view = new AccEditView(
          {
              model: __jsonModel,
              el: '#tab_view'
          });



    });





</script>

<script id="role_select_template" type="text/html">
    <@  _.each(content,function(o,e)  { console.log("**************") ;console.log(o.value); @>  
        <option value="<@= o.value @>" <@ if(selectedrole==o.value) { @> selected="selected" <@  } @> ><@= o.title @></option>
    <@ });@> 
</script>














