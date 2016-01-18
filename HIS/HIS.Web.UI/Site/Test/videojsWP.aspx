<%@ Page Title="" Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="False" CodeBehind="videojsWP.aspx.cs" Inherits="HIS.Web.UI.Site.Test.videojsWP" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        videojs.options.flash.swf = "app/StreamsFile/Get?filename=video-js.swf";
    </script>
    <video id="aaa" class="video-js vjs-default-skin" controls preload="none" width="640" height="264"
        poster=""
        data-setup="{}">
        <source src="/app/StreamsFile/Get?filename=023ae61c-d26b-4962-becd-a3be00b9e705.mp4" type='video/mp4' />

    </video>
</asp:Content>




















