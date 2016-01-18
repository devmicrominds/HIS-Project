<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Resources_Search_Gallery.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_Search_Gallery" %>

<section class="widget">
    <div class="tab-content">
        <div id="tab_view" class="tab-pane fade active in">
            <div class="body">
                <div id="search_gallery_form" class="form-horizontal">
                    <fieldset>
                        <legend style="padding-left: 10px;">Search Gallery Content</legend>
                    </fieldset>
                    <div class="form-actions">
                        <button data-action="return" type="button" class="btn btn-success">return </button>
                        <br />
                        <div id="resources_search_gallery" class="well">
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</section>

<script type="text/javascript">

    var ResourcesSearchGallery = Backbone.View.extend(
    {
    initialize: function (options) {
        var self = this;
        self.m_template = undefined;
        self.m_context = undefined;
        self._selectTemplate();
        self.path = options.path;



    },
    render: function () {

        var self = this;
        var snippet = _.template($(self.m_template).html(), {
            resources: self.model.SelectedData
        });
        $("#resources_search_gallery").append(snippet);


        self.m_context = self.model.MediaType;
        switch (self.m_context) {
            case 'Music':
                self._renderMusicContext();
                break;
            case 'Video':
                self._renderVideoContext();
                break;
            case 'Image':
                self._renderImageContext();
                break;


        }
    },
    events:
           {
               'click button[data-action=return]': '_return',

           },
    _return: function () {
        var self = this;
        var data = {
            Action: 'NavigateTo',
            Data: { Context: 'ClickSearch' },
            KeyInput: self.model.KeyInput,
        };
        amplify.publish('RESOURCE_MENU_ACTION_REQUEST', data);

    },
    _selectTemplate: function () {
        var self = this;
        self.m_context = self.model.MediaType;
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
    _renderVideoContext: function () {
        var self = this;
        videojs.options.flash.swf = self.path;
    },
    _renderImageContext: function () {
        console.log('render image context');
        var self = this;
        var imageGallery = self.$el.find('#image_gallery');
        $(image_gallery).lightGallery();
    },
});



    $(function () {

        __JsonModel(<%= this.JsonData %>);


        var resourcesSearchGallery = new ResourcesSearchGallery({
            el: "#tab_view",
            model: __jsonModel,
            path: "app/StreamsFile/Get?filename=video-js.swf"

        });

        resourcesSearchGallery.render();



    });
</script>


<script id="video_template" type="text/html">
    <div class="row">
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

                        </div>
                    </div>
                </li>
            <@ }); @>
        </ul>
    </div>
</script>

<script id="music_template" type="text/html">
    <div class="row">
        <div class="wrapper">

            <ul id="music_gallery" class="row thumbnails">
                <@ _.each(resources,function(o,e){  @>
                  <li class="col-md-12 col-sm-2 col-xs-3">
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


<script id="image_template" type="text/html">
    <div class="row">
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
