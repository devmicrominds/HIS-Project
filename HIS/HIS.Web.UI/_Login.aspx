<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="_Login.aspx.cs" Inherits="HIS.Web.UI.Login" %>
<!DOCTYPE html>
<html>
<head>
    <title>Light Blue - Admin Template</title>
    <link href="Contents/css/application.css" rel="stylesheet">
    <link rel="shortcut icon" href="img/favicon.png">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <meta charset="utf-8">
    <script src="Contents/lib/jquery/jquery-2.0.3.min.js" type="text/javascript"> </script>
    <script src="Contents/lib/jquery-pjax/jquery.pjax.js" type="text/javascript"></script>     
    <script src="Contents/lib/knockout/knockout-3.1.0.js" type="text/javascript"></script>
    <script src="Contents/js/settings.js" type="text/javascript"> </script>
    
 
</head>
<body class="background-dark">
    <div class="single-widget-container">
        <section class="widget login-widget">
            <header class="text-align-center">
                <h4>Login to your account</h4>
            </header>
            <div class="body">
                <form class="no-margin" data-bind="submit:doLogin" >
                    <fieldset>
                        <div class="form-group no-margin">
                            <label for="username">Username</label>
                            <div class="input-group input-group-lg">
                                <span class="input-group-addon">
                                    <i class="eicon-user"></i>
                                </span>
                                <input data-bind="value:username" id="username" type="text" class="form-control input-lg" placeholder="Your Username">
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="password">Password</label>
                            <div class="input-group input-group-lg">
                                <span class="input-group-addon">
                                    <i class="fa fa-lock"></i>
                                </span>
                                <input data-bind="value:password" id="password" type="password" class="form-control input-lg" placeholder="Your Password">
                            </div>

                        </div>
                        <legend></legend>
                    </fieldset>
                    <div class="form-actions">
                        <button type="submit" class="btn btn-block btn-lg btn-danger">
                            <span class="small-circle"><i class="fa fa-caret-right"></i></span>
                            <small>Sign In</small>
                        </button>
                        <div class="forgot"></div>
                    </div>
                </form>
            </div>
            <footer>
                <div class="facebook-login">
                    
                </div>
            </footer>
        </section>
    </div>
    <script type="text/javascript">

        $(function ()
        {
            function LoginViewModel(data) {
                var self = this;
                self.username = ko.observable();
                self.password = ko.observable();

                self.doLogin = function () {

                    var login = {};
                    login.Username = this.username();
                    login.Password = this.password();

                    $.ajax(
                    {
                        url: "app/Authorization/Post",
                        type: "POST",
                        contentType: "application/json;charset=utf-8",
                        data: JSON.stringify(login),
                        dataType: "json",
                        async: false,
                        success: function (response) {
                            if (response === true) {
                                window.location.href = 'Site/Home.aspx';
                            }
                            else {
                                alert('false');
                            }
                        },
                        error: function (x, e) {
                            alert('Failed');
                            alert(x.responseText);
                            alert(x.status);

                        }
                    });
                };
            };

            ko.applyBindings(new LoginViewModel());
        });

    </script>
</body>
</html>
