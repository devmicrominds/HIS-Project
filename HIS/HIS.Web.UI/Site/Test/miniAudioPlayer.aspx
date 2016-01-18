<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="miniAudioPlayer.aspx.cs" Inherits="HIS.Web.UI.Site.Test.miniAudioPlayer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery/jquery-2.1.1.min.js") %>"> </script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer02/inc/jquery.jplayer.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/musicPlayer02/inc/jquery.mb.miniPlayer.js") %>"></script>
    <link href="<%= Page.ResolveUrl("~/Contents/musicPlayer02/css/miniplayer.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(function () {



            $(".audio").mb_miniPlayer({
                width: 240,
                inLine: false,
                onEnd: playNext
            });

            function playNext(player) {
                var players = $(".audio");
                document.playerIDX = (player.idx <= players.length - 1 ? player.idx : 0);
                console.debug(document.playerIDX, player.idx)
                players.eq(document.playerIDX).mb_miniPlayer_play();
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">

            <a id="m1" class="audio {autoplay:true, inLine:true}"
                href="http://www.pacdv.com/sounds/ambience_sounds/airport-gate-1.mp3">Airport Gate (Boarding)</a>
            <button onclick="$('#m1').mb_miniPlayer_stop()">stop</button>
            <button onclick="$('#m1').mb_miniPlayer_play()">play</button>
            <hr>
            <a id="m2" class="audio {ogg:'http://www.miaowmusic.com/ogg/Miaow-02-Hidden.ogg'}"
                href="http://www.pacdv.com/sounds/ambience_sounds/g-t-1.mp3">Group Talking</a>
            <hr>
            <a id="m3" class="audio {ogg:'http://www.miaowmusic.com/ogg/Miaow-02-Hidden.ogg'}"
                href="http://www.pacdv.com/sounds/ambience_sounds/water-stream-1.mp3" style="display: none">Water Stream
        (Small)</a>
            <hr>
            <a id="m4" class="audio {ogg:'http://www.miaowmusic.com/ogg/Miaow-02-Hidden.ogg'}"
                href="http://www.pacdv.com/sounds/domestic_sound_effects/doorbell-1.mp3">Doorbell</a>
            <hr>
            <a id="m5" class="audio {ogg:'http://www.miaowmusic.com/ogg/Miaow-02-Hidden.ogg'}"
                href="http://www.pacdv.com/sounds/transportation_sounds/antique-car-honk-1.mp3" style="display: none">Antique Car
        & Honk</a>





        </div>
    </form>
</body>
</html>
