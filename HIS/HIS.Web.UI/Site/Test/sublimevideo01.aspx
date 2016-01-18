<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sublimevideo01.aspx.cs" Inherits="HIS.Web.UI.Site.Test.sublimevideo01" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Contents/lib/jquery/jquery-2.1.1.min.js") %>"> </script>

    <title></title>
    <style>
        .sv_playlist {
            width: 575px;
            height: 245px;
            padding: 13px;
            float: left;
            margin-right: 40px;
            background: #c5d0d9;
        }

            .sv_playlist .video_wrap {
                width: 768px;
                height: 432px;
                display: none;
            }

            .sv_playlist .video_wrap {
                width: 432px;
                height: 243px;
                display: none;
                float: left;
                background: #fff;
                padding: 1px;
            }

                .sv_playlist .video_wrap.active {
                    display: block;
                }

            .sv_playlist ul.thumbs {
                list-style-type: none;
                width: 180px;
                float: left;
            }

            .sv_playlist ul.thumbs {
                width: 139px;
            }

            .sv_playlist li {
                display: block;
                width: 144px;
                height: 81px;
                margin-right: 16px;
                margin-bottom: 34px;
                background: #000;
                border: 1px solid #000;
            }

            .sv_playlist li {
                width: 127px;
                height: 71px;
                margin: 0 0 13px 12px;
            }

                .sv_playlist li.active {
                    border-color: #fff;
                }

                .sv_playlist li a {
                    display: block;
                    position: relative;
                }

                    .sv_playlist li a span.play {
                        display: block;
                        width: 144px;
                        height: 81px;
                        background: url(http://media.jilion.com/images/playlist_play_icon.png) no-repeat center;
                        background-color: rgba(0,0,0,0.6);
                        position: absolute;
                        top: 0;
                        left: 0;
                    }

                    .sv_playlist li a span.play {
                        width: 127px;
                        height: 71px;
                    }

                    .sv_playlist li a:hover span.play {
                        background-color: rgba(0,0,0,0);
                    }

                .sv_playlist li.active a span.play {
                    background: none;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="playlist1" class='sv_playlist'>
            <div class='video_wrap'>
                <video id='video1' poster='http://media.sublimevideo.net/v/midnight_sun_sv1_2_800.jpg' width='432' height='243' preload='none'>
                    <source src='http://localhost:56061/app/StreamsFile/Get?filename=2dcf77d2-2db7-481a-8f0a-a3be00b9e192.mp4' />

                </video>
            </div>
            <div class='video_wrap'>
                <video id='video2' poster='http://media.sublimevideo.net/v/midnight_sun_sv1_3_800.jpg' width='432' height='243' preload='none'>
                    <source src='http://localhost:56061/app/StreamsFile/Get?filename=2dcf77d2-2db7-481a-8f0a-a3be00b9e192.mp4' />

                </video>
            </div>
            <div class='video_wrap'>
                <video id='video3' poster='http://media.sublimevideo.net/v/midnight_sun_sv1_4_800.jpg' width='432' height='243' preload='none'>
                    <source src='http://media.sublimevideo.net/v/midnight_sun_sv1_4_360p.mp4' />

                </video>
            </div>
            <ul class='thumbs'>
                <li id='thumbnail_video1'>
                    <a href=''>
                        <img alt='' src='http://media.sublimevideo.net/v/midnight_sun_sv1_2_144.jpg' width='127' height='71' />
                        <span class='play' />
                    </a>
                </li>
                <li id='thumbnail_video2'>
                    <a href=''>
                        <img alt='' src='http://media.sublimevideo.net/v/midnight_sun_sv1_3_144.jpg' width='127' height='71' />
                        <span class='play' />
                    </a>
                </li>
                <li id='thumbnail_video3' class='last_thumbnail'>
                    <a href=''>
                        <img alt='' src='http://media.sublimevideo.net/v/midnight_sun_sv1_4_144.jpg' width='127' height='71' />
                        <span class='play' />
                    </a>
                </li>
            </ul>
        </div>

        <script>

            var SublimeVideo = SublimeVideo || { playlists: {} };

            $(document).ready(function () {
                var playlistOptions = { autoplayOnPageLoad: false, loadNext: true, autoplayNext: true, loop: false };

                $('.sv_playlist').each(function (i, el) {
                    SublimeVideo.playlists[el.id] = new SublimeVideoPlaylist(el.id, playlistOptions);
                });
            });

            var SublimeVideoPlaylist = function (playlistWrapperId, options) {
                if (!$("#" + playlistWrapperId)) return;

                this.options = options;
                this.playlistWrapperId = playlistWrapperId;
                this.firstVideoId = $("#" + this.playlistWrapperId + " video").attr("id");

                this.setupObservers();
                this.updateActiveVideo(this.firstVideoId);
            };

            $.extend(SublimeVideoPlaylist.prototype, {
                setupObservers: function () {
                    var that = this;

                    $("#" + this.playlistWrapperId + " li").each(function () {
                        $(this).click(function (event) {
                            event.preventDefault();

                            if (!$(this).hasClass("active")) {
                                that.clickOnThumbnail($(this).attr("id"), that.options['autoplayNext']);
                            }
                        });
                    });
                },
                reset: function () {
                    $("#" + this.playlistWrapperId + " .video_wrap.active").removeClass("active");

                    sublime.unprepare($("#" + this.activeVideoId)[0]);
                    this.deselectThumbnail(this.activeVideoId);
                },
                clickOnThumbnail: function (thumbnailId, autoplay) {
                    var that = this;

                    this.updateActiveVideo(thumbnailId.replace(/^thumbnail_/, ""));

                    sublime.prepare($("#" + this.activeVideoId)[0], function (player) {
                        if (autoplay) player.play();
                        player.on({
                            start: that.onVideoStart,
                            play: that.onVideoPlay,
                            pause: that.onVideoPause,
                            end: that.onVideoEnd,
                            stop: that.onVideoStop
                        });
                    });
                },
                selectThumbnail: function (videoId) {
                    $("#thumbnail_" + videoId).addClass("active");
                },
                deselectThumbnail: function (videoId) {
                    $("#thumbnail_" + videoId).removeClass("active");
                },
                updateActiveVideo: function (videoId) {

                    if (this.activeVideoId) this.reset();


                    this.activeVideoId = videoId;


                    this.showActiveVideo();
                },
                showActiveVideo: function () {

                    this.selectThumbnail(this.activeVideoId);


                    $("#" + this.activeVideoId).parent().addClass("active");
                },
                handleAutoNext: function (newVideoId) {
                    this.clickOnThumbnail(newVideoId, this.options['autoplayNext']);
                },
                onVideoStart: function (player) {

                },
                onVideoPlay: function (player) {

                },
                onVideoPause: function (player) {

                },
                onVideoEnd: function (player) {

                    var videoId = player.videoElement().id;
                    if (videoId.match(/^video([0-9]+)$/) !== undefined) {
                        var playlistId = $(player.videoElement()).parents('.sv_playlist').attr("id");
                        var nextThumbnail = $("#thumbnail_" + videoId).next("li");

                        if (nextThumbnail.length > 0) {
                            if (SublimeVideo.playlists[playlistId].options['loadNext']) {
                                SublimeVideo.playlists[playlistId].handleAutoNext(nextThumbnail.attr("id"));
                            }
                        }
                        else if (SublimeVideo.playlists[playlistId].options['loop']) {
                            SublimeVideo.playlists[playlistId].updateActiveVideo(SublimeVideo.playlists[playlistId].firstVideoId);
                            SublimeVideo.playlists[playlistId].handleAutoNext(SublimeVideo.playlists[playlistId].activeVideoId);
                        }
                        else { player.stop(); }
                    }
                },
                onVideoStop: function (player) {

                }
            });

            sublime.ready(function () {
                for (var playlistId in SublimeVideo.playlists) {
                    SublimeVideo.playlists[playlistId].clickOnThumbnail(SublimeVideo.playlists[playlistId].activeVideoId, SublimeVideo.playlists[playlistId].options['autoplayOnPageLoad']);
                }
            });
        </script>
    </form>
</body>

</html>
