<%@ Page Title="" Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="False" CodeBehind="TestWithWP.aspx.cs" Inherits="HIS.Web.UI.Site.Test.WebForm1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

</asp:Content>




















