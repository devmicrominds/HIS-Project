<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test01.aspx.cs" Inherits="HIS.Web.UI.Site.Test.Test01" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery/jquery-2.1.1.min.js") %>"> </script>
    <link href="<%= Page.ResolveUrl("~/Contents/musicPlayer03/plugin/css/style.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Page.ResolveUrl("~/Contents/musicPlayer03/css/demo.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer03/plugin/jquery-jplayer/jquery.jplayer.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer03/plugin/ttw-music-player-min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer03/js/myplaylist.js") %>"></script>
</head>
<body>
    <script type="text/javascript">
        $(document).ready(function () {
            var description = 'TEST';
            $('body').ttwMusicPlayer(myPlaylist, {
                autoPlay: false,
                description: description,
                jPlayer: {
                    //swfPath:'/plugin/jquery-jplayer' //You need to override the default swf path any time the directory structure changes
                }
            });
        });
    </script>
</body>
</html>
