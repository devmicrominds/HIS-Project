<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jdialog.aspx.cs" Inherits="HIS.Web.UI.Site.Test.jdialog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery/jquery-2.1.1.min.js") %>"> </script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/backbone/underscore-min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/backbone/backbone-min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/backbone/backbone-controller.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery-ui-1.10.3.custom.js") %>"></script>

</head>
<body>
    <div id="myModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <!-- dialog body -->
                <div class="modal-body">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    Hello world!
                </div>
                <!-- dialog buttons -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary">OK</button></div>
            </div>
        </div>
    </div>
    <script>
        $("#myModal").on("show", function () { // wire up the OK button to dismiss the modal when shown
            $("#myModal a.btn").on("click", function (e) {
                console.log("button pressed"); // just as an example...
                $("#myModal").modal('hide'); // dismiss the dialog
            });
        });

        $("#myModal").on("hide", function () { // remove the event listeners when the dialog is dismissed
            $("#myModal a.btn").off("click");
        });
        $("#myModal").on("hidden", function () { // remove the actual elements from the DOM when fully hidden
            $("#myModal").remove();
        });
        $("#myModal").modal({ // wire up the actual modal functionality and show the dialog
            "backdrop": "static",
            "keyboard": true,
            "show": true // ensure the modal is shown immediately
        });
    </script>
</body>
</html>
