<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="debounce.aspx.cs" Inherits="HIS.Web.UI.Site.Test.debounce" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery/jquery-2.1.1.min.js") %>"> </script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/backbone/underscore-min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/backbone/backbone-min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/backbone/backbone-controller.js") %>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input id="searchText" class="text" type="text" name="whatever">
        </div>
    </form>
    <p id="txtview01"></p>
    <p id="txtview02"></p>
    <p id="P1"></p>

    <script type="text/javascript">
        $(function () {

            var default_text = $('#text-type').text(),
            text_counter_1 = 0,
            text_counter_2 = 0;


            function text_1() {
                var val = $(this).val(),
                html = 'Not-debounced AJAX request executed: ' + text_counter_1++ + ' times.'
                + (val ? ' Text: ' + val : '');

                $('#txtview01').html(html);
            };

            function text_2() {
                var val = $(this).val(),
                html = 'Debounced AJAX request executed: ' + text_counter_2++ + ' times.'
                + (val ? ' Text: ' + val : '');

                $('#txtview02').html(html);
            };


            $('#searchText').keyup(text_1);


            $('#searchText').keyup(_.debounce(text_2, 500));


            //text_1();
            //text_2();
        });
    </script>
</body>
</html>
