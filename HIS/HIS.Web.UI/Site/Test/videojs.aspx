<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="videojs.aspx.cs" Inherits="HIS.Web.UI.Site.Test.videojs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery/jquery-2.1.1.min.js") %>"> </script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/videojs/video.js") %>"></script>
    <link href="<%= Page.ResolveUrl("~/Contents/videojs/video-js.css") %>" rel="stylesheet" type="text/css" />

      <script>
          videojs.options.flash.swf = "app/StreamsFile/Get?filename=video-js.swf";
  </script>
</head>
<body>
    <video id="example_video_1" class="video-js vjs-default-skin" controls preload="none" width="640" height="264"
        poster=""
        data-setup="{}">
        <source src="/app/StreamsFile/Get?filename=023ae61c-d26b-4962-becd-a3be00b9e705.mp4" type='video/mp4' />
     
        <track kind="captions" src="demo.captions.vtt" srclang="en" label="English"></track><!-- Tracks need an ending tag thanks to IE9 -->
        <track kind="subtitles" src="demo.captions.vtt" srclang="en" label="English"></track><!-- Tracks need an ending tag thanks to IE9 -->
        <p class="vjs-no-js">To view this video please enable JavaScript, and consider upgrading to a web browser that <a href="http://videojs.com/html5-video-support/" target="_blank">supports HTML5 video</a></p>
    </video>
</body>


</html>
