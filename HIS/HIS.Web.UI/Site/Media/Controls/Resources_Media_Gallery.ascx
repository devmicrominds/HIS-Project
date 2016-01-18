<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Resources_Media_Gallery.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_Media_Gallery" %>
<div id="resources_media_gallery" class="well">
</div>
<script type="text/javascript">

    var ResourcesMediaGallery = Backbone.View.extend(
    {
        initialize: function () {
            var self = this;
            self.m_template = undefined;
            self.m_context = undefined;
            self._selectTemplate();

        },
        render: function () {
            var self = this;
            var snippet = _.template($(self.m_template).html(), {
                resources: self.model.Data.Resources
            });
            self.$el.append(snippet);
            console.log(self.m_context);
            switch (self.m_context) {
                case 'Image':
                    self._renderImageContext();
                    break;
                case 'Video':
                    self._renderVideoContext();
                    break;
                case 'Music':
                    self._renderMusicContext();
                    break;
            }

        },
        _selectTemplate: function () {
            var self = this;
            self.m_context = self.model.Data.Context;
            switch (self.m_context) {
                case 'Image':
                    self.m_template = '#image_template';
                    break;
                case 'Video':
                    self.m_template = '#video_template';
                    break;
                case 'Music':
                    self.m_template = '#music_template';
                    break;
            }
        },
        _renderImageContext: function () {
            console.log('render image context');
            var self = this;
            var imageGallery = self.$el.find('#image_gallery');
            $(image_gallery).lightGallery();
        },

        _renderVideoContext: function () {
            console.log('render video context');
            var self = this;
            /*var videoGallery = self.$el.find('#video_gallery');
            $(video_gallery).lightGallery();*/
        },

        _renderMusicContext: function () {
            console.log('render Music context');
            var self = this;
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


        },
    });

    $(function () {




        __JsonModel(<%= this.JsonData %>);

        console.log(__jsonModel);

        var resourceMediaGallery = new ResourcesMediaGallery({
            el: "#resources_media_gallery",
            model: __jsonModel,

        });

        resourceMediaGallery.render();



    });



</script>




<script id="image_template" type="text/html">
    <@ if(resources.length!=0) {@>
             <h4 style="margin-left:-15px; margin-top:-5px;">Image Gallery</h4> <@ } @>
    <hr />
    <div class="row">
        <@ if(resources.length==0) {@>
            <h4 style="text-align:center; font-size:20px;"><strong>No file uploaded</strong></h4>
        <@ } @>
        <ul id="image_gallery" class="row thumbnails">
            <@ _.each(resources,function(o,e){  @>
                <li class="col-md-2 col-sm-2 col-xs-3" data-src="/app/media/getimage?imageId=<@= o.Id @>">
                    <div class="thumbnail">
                        <img alt="" src="data:image/jpg;base64,<@= o.Thumbnail @>" />
                        <div class="caption">
                            <h5><@= o.Name @></h5>
                            <small><strong><@= o.Description @></strong></small>
                            <p class="text-right">
                                <small><b><@= o.Dimension @></b></small>
                                <br>
                                <small><em><@= o.Size @></em></small>
                            </p>
                        </div>
                    </div>
                </li>
            <@ }); @>
        </ul>
    </div>
</script>


<script id="video_template" type="text/html">
    <@ if(resources.length!=0) {@>
             <h4 style="margin-left:-15px; margin-top:-5px;">Video Gallery</h4> <@ } @>
    <hr />
    <div class="row">
        <@ if(resources.length==0) {@>
            <h4 style="text-align:center; font-size:20px;"><strong>No file uploaded</strong></h4>
        <@ } @>
        <ul id="video_gallery" class="row thumbnails">
            
            <@ _.each(resources,function(o,e){  @>
                <li class="col-md-6 col-sm-2 col-xs-3">
                    <div class="thumbnail">

                        <video width="330" height="320" controls preload class="mejs-player"
                            data-mejsoptions='{"alwaysShowControls": true}'>
                            <source src="/app/StreamsFile/Get?filename=<@= o.FileName @>" type="video/mp4">
                        </video>
                        <div class="caption">
                            <h5><@= o.Name @></h5>
                            <small><strong><@= o.Description @></strong></small>
                            <p class="text-right">
                                <small><b><@= o.Dimension @></b></small>
                                <br>
                                <small><em><@= o.Size @></em></small>
                            </p>
                        </div>
                    </div>
                </li>
            <@ }); @>
        </ul>
    </div>
</script>





<script id="music_template" type="text/html">
     <@ if(resources.length!=0) {@>
             <h4 style="margin-left:-15px; margin-top:-5px;">Music Gallery</h4> <@ } @>
    <hr />
    <div class="row">
        <div class="wrapper">
            <@ if(resources.length==0) {@>
            <h4 style="text-align:center; font-size:20px;"><strong>No file uploaded</strong></h4>
        <@ } @>
            <ul id="music_gallery" class="row thumbnails">
                <@ _.each(resources,function(o,e){  @>
                  <li class="col-md-6 col-sm-2 col-xs-3">
                      <div class="thumbnail">

                          <a id='<@= o.Id @>' class="audio {ogg:'http://www.miaowmusic.com/ogg/Miaow-02-Hidden.ogg'}"
                              href="/app/StreamsFile/Get?filename=<@= o.FileName @>" style="display: none"><@= o.Name @> </a>
                      </div>
                      <div class="caption">
                          <h5><@= o.Name @></h5>
                          <small><strong><@= o.Artist @></strong></small>
                          <p class="text-center">
                              <small><b><@= o.Album @></b></small>
                              <br>
                              <small><em><@= o.Genre @></em></small>
                              <br>
                              <small><em><@= o.Size @></em></small>
                              <br>
                              <small><em><@= o.Length @></em></small>
                          </p>
                      </div>
                  </li>
                <@ }); @>
            </ul>



        </div>
    </div>
</script>



<%--<script id="music_template" type="text/html">
    <div class="row">
        <table border="1">
            <@ _.each(resources,function(o,e){  @>
                <tr>
                    <td width="30%">

                        <audio controls="controls">
                            <source src="/app/StreamsFile/Get?filename=<@= o.FileName @>" />

                        </audio>

                    </td>
                    <td width="70%">
                        <div class="caption">
                            <h5><@= o.Name @></h5>
                            <small><strong><@= o.Artist @></strong></small>
                            <p class="text-center">
                                <small><b><@= o.Album @></b></small>
                                <br>
                                <small><em><@= o.Genre @></em></small>
                                <br>
                                <small><em><@= o.Size @></em></small>
                                <br>
                                <small><em><@= o.Length @></em></small>
                            </p>
                        </div>
                    </td>
                </tr>


            <@ }); @>
        </table>
    </div>
</script>--%>



<%--<script id="video_template" type="text/html">
    <div class="row">

        <div class="box-wrapper  span1">
            <div class="row">
                <div class="title span10">
                    <h3 class="pull-left">Lorem ipsum dolor ...</h3>
                    <div class="sort pull-right dropdown">
                        <a class="dropdown-toggle" data-toggle="dropdown" href="#">Most Viewed
								<b class="caret"></b>
                        </a>
                        <ul class="dropdown-menu">
                            <li><a href="#"><i class="icon-tag"></i>By Name</a></li>
                            <li><a href="#"><i class="icon-list"></i>List</a></li>
                            <li><a href="#"><i class="icon-eye-open"></i>View</a></li>
                        </ul>
                    </div>
                </div>
                <!-- end title -->
            </div>
            <ul class="thumbnails thumbnails-vertical">
                <@ _.each(resources,function(o,e){  @>
                <li class="span5">

                    <div class="thumbnail border-radius-top">
                        <div class="bg-thumbnail-img">
                            <video width="500" height="350" controls>
                                <source src="/app/StreamsFile/Get?filename=<@= o.FileName @>" type="video/mp4">
                            </video>
                        </div>
                        <div class="thumbnail-content-left">
                            <h5><a href="detail.html"><@= o.Name @></a></h5>
                            <p>
                                <@= o.Description @>
                            </p>
                        </div>

                    </div>
                    <div class="box border-radius-bottom">
                        <p>
                            <span class="title_torrent pull-left pull-left"><@= o.Name @></span>
                            <span class="number-view pull-right"><i class="icon-white icon-eye-open"></i><@= o.Size @></span>
                        </p>
                    </div>
                </li>


                <@ }); @>
            </ul>

        </div>
    </div>
</script>--%>





<%--<script id="video_template" type="text/html">
    <div class="row">

        <div class="box-wrapper  span1">
            <div class="row">
                <div class="title span10">
                    <h3 class="pull-left">Lorem ipsum dolor ...</h3>
                    <div class="sort pull-right dropdown">
                        <a class="dropdown-toggle" data-toggle="dropdown" href="#">Most Viewed
								<b class="caret"></b>
                        </a>
                        <ul class="dropdown-menu">
                            <li><a href="#"><i class="icon-tag"></i>By Name</a></li>
                            <li><a href="#"><i class="icon-list"></i>List</a></li>
                            <li><a href="#"><i class="icon-eye-open"></i>View</a></li>
                        </ul>
                    </div>
                </div>
                <!-- end title -->
            </div>
            <ul class="thumbnails thumbnails-vertical">
                <@ _.each(resources,function(o,e){  @>
                <li class="span5">

                    <div class="thumbnail border-radius-top">
                        <div class="bg-thumbnail-img">
                            <video width="500" height="350" controls>
                                <source src="http://localhost:15268/api/Videos/mp4/<@= o.FileName @>" type="video/mp4">
                            </video>
                        </div>
                        <div class="thumbnail-content-left">
                            <h5><a href="detail.html"><@= o.Name @></a></h5>
                            <p>
                                <@= o.Description @>
                            </p>
                        </div>

                    </div>
                    <div class="box border-radius-bottom">
                        <p>
                            <span class="title_torrent pull-left pull-left"><@= o.Name @></span>
                            <span class="number-view pull-right"><i class="icon-white icon-eye-open"></i><@= o.Size @></span>
                        </p>
                    </div>
                </li>


                <@ }); @>
            </ul>

        </div>
    </div>
</script>--%>


<%--
<script id="Script1" type="text/html">
    <div class="row">
        <table>
            <@ _.each(resources,function(o,e){  @>
                <tr>
                    <td>
                        <div class="flex-video widescreen">
                            <video width="320" height="240" controls>
                                <source src="http://localhost:15268/api/Videos/mp4/<@= o.FileName @>" type="video/mp4">
                            </video>
                        </div>
                    </td>
                    <td>
                        <div class="caption">
                            <h5><@= o.Name @></h5>
                            <small><strong><@= o.Description @></strong></small>
                            <p class="text-right">
                                <small><b><@= o.Dimension @></b></small>
                                <br>
                                <small><em><@= o.Size @></em></small>
                            </p>
                        </div>
                    </td>
                </tr>


            <@ }); @>
        </table>
    </div>
</script>--%>
