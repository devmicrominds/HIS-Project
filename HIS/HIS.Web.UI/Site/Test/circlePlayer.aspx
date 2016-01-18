<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="circlePlayer.aspx.cs" Inherits="HIS.Web.UI.Site.Test.circlePlayer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery/jquery-2.1.1.min.js") %>"> </script>
    <link href="<%= Page.ResolveUrl("~/Contents/musicPlayer/css/not.the.skin.css") %>" rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/Contents/musicPlayer/circle.skin/circle.player.css") %>" rel="stylesheet" />
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer/js/jquery.transform2d.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer/js/jquery.grab.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer/js/jquery.jplayer.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer/js/mod.csstransforms.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer/js/circle.player.js") %>"></script>
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {

            /*
             * Instance CirclePlayer inside jQuery doc ready
             *
             * CirclePlayer(jPlayerSelector, media, options)
             *   jPlayerSelector: String - The css selector of the jPlayer div.
             *   media: Object - The media object used in jPlayer("setMedia",media).
             *   options: Object - The jPlayer options.
             *
             * Multiple instances must set the cssSelectorAncestor in the jPlayer options. Defaults to "#cp_container_1" in CirclePlayer.
             */

            var myCirclePlayer = new CirclePlayer("#jquery_jplayer_1",
            {
                m4a: "http://www.jplayer.org/audio/m4a/Miaow-07-Bubble.m4a",
                oga: "http://www.jplayer.org/audio/ogg/Miaow-07-Bubble.ogg"
            }, {
                cssSelectorAncestor: "#cp_container_1"
            });

            // This code creates a 2nd instance. Delete if not required.

            var myOtherOne = new CirclePlayer("#jquery_jplayer_2",
            {
                m4a: "http://www.jplayer.org/audio/m4a/Miaow-04-Lismore.m4a",
                oga: "http://www.jplayer.org/audio/ogg/Miaow-04-Lismore.ogg"
            }, {
                cssSelectorAncestor: "#cp_container_2"
            });
        });
		</script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="jquery_jplayer_1" class="cp-jplayer"></div>

        <!-- This is the 2nd instance's jPlayer div -->
        <div id="jquery_jplayer_2" class="cp-jplayer"></div>

        <div class="prototype-wrapper">
            <!-- A wrapper to emulate use in a webpage and center align -->


            <!-- The container for the interface can go where you want to display it. Show and hide it as you need. -->

            <div id="cp_container_1" class="cp-container">
                <div class="cp-buffer-holder">
                    <!-- .cp-gt50 only needed when buffer is > than 50% -->
                    <div class="cp-buffer-1"></div>
                    <div class="cp-buffer-2"></div>
                </div>
                <div class="cp-progress-holder">
                    <!-- .cp-gt50 only needed when progress is > than 50% -->
                    <div class="cp-progress-1"></div>
                    <div class="cp-progress-2"></div>
                </div>
                <div class="cp-circle-control"></div>
                <ul class="cp-controls">
                    <li><a class="cp-play" tabindex="1">play</a></li>
                    <li><a class="cp-pause" style="display: none;" tabindex="1">pause</a></li>
                    <!-- Needs the inline style here, or jQuery.show() uses display:inline instead of display:block -->
                </ul>
            </div>

            <!-- This is the 2nd instance HTML -->

            <div id="cp_container_2" class="cp-container">
                <div class="cp-buffer-holder">
                    <div class="cp-buffer-1"></div>
                    <div class="cp-buffer-2"></div>
                </div>
                <div class="cp-progress-holder">
                    <div class="cp-progress-1"></div>
                    <div class="cp-progress-2"></div>
                </div>
                <div class="cp-circle-control"></div>
                <ul class="cp-controls">
                    <li><a class="cp-play" tabindex="1">play</a></li>
                    <li><a class="cp-pause" style="display: none;" tabindex="1">pause</a></li>
                </ul>
            </div>

        </div>
    </form>
</body>
</html>
