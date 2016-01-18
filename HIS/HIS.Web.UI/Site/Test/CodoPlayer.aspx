<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CodoPlayer.aspx.cs" Inherits="HIS.Web.UI.Site.Test.CodoPlayer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery/jquery-2.1.1.min.js") %>"> </script>
      <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery/CodoPlayer.js") %>"> </script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayerCodo/CodoPlayer.js") %>"></script>
    <script>
      

        $(document).ready(function () {
            CodoPlayer({
                title: "Title here",
                poster: "app/StreamsFile/Get?filename=652b69c4-0e52-49d8-81d5-a3b90108d01b.jpg",
                src: "relative://app/StreamsFile/Get?filename=2dcf77d2-2db7-481a-8f0a-a3be00b9e192.mp4" // Absolute URL recommended. For example "http://example/video.mp4"
            }, {
                width: 600, // Remove width & height keys for responsive behaviour
                height: 338,
                volume: 80,
                preload: false
                /* For more settings visit http://codoplayer.com/configuration/ */
            });
        });
		</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>


</html>
