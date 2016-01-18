<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Resources_Menu.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_Menu" %>
<div class="row">
    <div class="col-md-12">
        <section class="widget">
            <div class="row">
                <div id="resources_menu" class="panel col-md-1">
                    <div>
                        <div class="header">
                            <h4>Media
                            </h4>
                        </div>
                    </div>
                    <div class="panel-body">
                        <ul class="news-list">
                            <li id="li_image" data-target="Image">
                                <span class="pull-left">
                                    <i class="eicon-picture" style="font-size: 20px;"></i>
                                </span>
                            </li>
                            <li data-target="Video">
                                <span class="pull-left">
                                    <i class="eicon-video " style="font-size: 20px;"></i>
                                </span>

                            </li>
                            <li data-target="Stream">
                                <span class="pull-left"><i class="fa fa-video-camera " style="font-size: 20px;"></i></span>
                            </li>
                            <li data-target="HTML5">
                                <span class="pull-left">
                                    <i class="fa fa-html5" style="font-size: 20px;"></i>
                                </span>
                            </li>
                            <li data-target="Music">
                                <span class="pull-left">
                                    <i class="fa fa-music" style="font-size: 20px;"></i>
                                </span>
                            </li>
                            <li data-target="Ticker">
                                <span class="pull-left">
                                    <i class="fa fa-comments" style="font-size: 20px;"></i>
                                </span>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="col-md-11">
                    <asp:PlaceHolder ID="MediaPlaceHolder" runat="server"></asp:PlaceHolder>
                </div>

            </div>
        </section>
    </div>
</div>
<script type="text/javascript">

    var ResourcesMenu = Backbone.View.extend(
    {
        initialize: function () {
            var self = this;
            console.log('initialize #');
            self._listenToEvents();


        },
        events: {
            'click .news-list > li': 'navigateTo'
        },
        navigateTo: function (e) {
            var li = $(e.target).closest('li');
            var target = li.data('target');
            var Json =
            {
                Action: 'NavigateTo',
                Data: {
                    Context: target
                },
            };
            _Post(document.URL, Json);

        },
        _render: function () {
            var self = this;
            if (_.isUndefined(self.model.Data))
                return;
            var context = self.model.Data.Context;
            self.activateListElement(context);

        },
        activateListElement: function (data) {
            var self = this;
            var element = this.$el.find('li[data-target="' + data + '"]');
            element.addClass('resource_list_active');
        },
        _listenToEvents: function () {
            var self = this;
            amplify.subscribe('RESOURCE_MENU_ACTION_REQUEST', self._resourceMenuActionRequest);

        },
        _resourceMenuActionRequest: function (e) {
            _Post(document.URL, e);
            return false;
        }

    });



    $(function () {

        __JsonModel(<%= this.JsonData %>);
        var resourcesMenu = new ResourcesMenu({
            el: '#resources_menu',
            model: __jsonModel
        });

        resourcesMenu._render();

    });

</script>

