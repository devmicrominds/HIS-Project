<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site/Base.Master" CodeBehind="miniAudioPlayerMP.aspx.cs" Inherits="HIS.Web.UI.Site.Test.miniAudioPlayerMP" %>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                console.debug(document.playerIDX, player.idx);
                players.eq(document.playerIDX).mb_miniPlayer_play();
            }
        });
    </script>
</asp:Content>
