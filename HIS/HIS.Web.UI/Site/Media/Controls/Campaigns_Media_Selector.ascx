<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Campaigns_Media_Selector.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Campaigns_Media_Selector" %>
 
<div id="media_selector_view" class="row">
    <div class="col-md-9">
        <section class="widget large" style="min-height: 500px;">
            <header>
                <h5>Resources </h5>
                <div class="actions">
                    <button type="button" data-action="show_editor" class="btn btn-primary">
                        Return
                    </button>
                </div>
            </header>
            <div class="body">
                <div class="row">
                    <div class="col-md-3">
                        <div class="mediainfo" style="min-height: 450px;">
                            <div class="wrapper">
                                <div class="vertical-line"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-9">
                        <div class="resources_selection">
                            <div id="m_select">
                                <%--<select name="media"></select>--%>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </section>
    </div>
    <div class="col-md-3">
        <section class="widget large" style="min-height: 500px;">
            <header>
                <h5>Selections</h5>
            </header>
            <div class="body">
                <div class="mediainfo" style="min-height: 450px;">
                    <div class="selected_items">
                        <ul class="news-list">
                            <li>
                                <div class="news-item-info">
                                </div>
                            </li>
                            <li>
                                <div class="news-item-info">
                                </div>
                            </li>
                            <li>
                                <div class="news-item-info">
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </section>
    </div>

</div>
<script type="text/javascript">

    __JsonModel(<%= this.JsonData %>);
    console.log(__jsonModel);

    var CampaignsMediaSelector = Backbone.View.extend({
        initialize: function () {
            var self = this;
            self.m_mediaselection = {}; 
        },
        events: {
            'click a[data-action=select_category]': '_selectCategory',
            'click button[data-action=show_editor]':'_showEditor',
        },
        _showEditor: function () {
            var self = this; 
            amplify.request(Backbone.REQUEST.ADD_CHANNEL_BLOCK,
               JSON.stringify(
           {
               Data:
               {
                   DateTimeRequest: moment().format(),
                   Channel: self.model.Channel,
                   Context: self.model.Context,
                   Collections: self.m_mediaselection,
               }

           }), function (data) {

               var self = this;
                var oJson =
                {
                    Action: 'EditCampaign',
                    DataTarget: data.Campaign,
                    Timeline: data.Timeline,
                    Channel:data.Channel,
                };
                _Post(document.URL, oJson);

           });

            

        }, 
        _selectCategory: function (e) {

            var self = this;
            var target = e.target;
            var context = self.model.Context;
            var mediaCategoryId = $(target).prop('id');

            amplify.request(Backbone.REQUEST.GET_BLOCK_RESOURCES,
            JSON.stringify({ Context: context, mediaCategoryId: mediaCategoryId }), function (data) {
                 
                switch (context) {
                    case "Image":
                        self._handleImages(data);
                        break;
                    case "Video":
                       
                        break;
                    case "HTML5":
                        self._handleHTML(data);
                        break;
                }

            });


        },
        _handleImages: function (data) {

            var self = this;
            var $selections = self.$el.find('.resources_selection');
            var snippet = _.template($('#image_resources_template').html(), {
                mediaresources: data.resources
            });
            self.m_mediaselection = {};
            self.buildSelected();
            $selections.empty();
            $selections.append(snippet);

            var select = $($selections).find('#image_select'); 
            $(select).MultiColumnSelect(
            {
                multiple: true,
                menuclass: 'mcs',
                openmenuClass: 'mcs-open',
                openmenuText: 'Choose ...',
                containerClass: 'mcs-container',
                itemClass: 'mcs-item', duration: 100,
                useOptionText: true,
                onItemSelect: function () {
                    var $elx = $(this);
                    var $eli = $elx.find('a');
                    var oid = $eli.data('id');
                    var name = $eli.data('name');
                    if (!_.isUndefined(oid)) {
                        if ($elx.hasClass('active'))
                        {
                            self.m_mediaselection[oid] =
                            {
                                ResourceId: oid,
                                Name: name,
                                Length: 4 /* hardcode image length = 4 */
                            };

                        }
                        else {
                            delete self.m_mediaselection[oid];
                        }


                        self.buildSelected();


                    }


                },

            });

            $('a.mcs-open').trigger('click');
        },
        _handleHTML: function (data) {
            var self = this;
            var $selections = self.$el.find('.resources_selection');
            var snippet = _.template($('#html_resources_template').html(), {
                mediaresources: data.resources
            });
            self.m_mediaselection = {};
            self.buildSelected();
            $selections.empty();
            $selections.append(snippet);
            var select = $($selections).find('#html_select');
             
            $(select).MultiColumnSelect(
            {
               multiple: true,
               menuclass: 'mcs',
               openmenuClass: 'mcs-open',
               openmenuText: 'Choose ...',
               containerClass: 'mcs-container',
               itemClass: 'mcs-item', duration: 100,
               useOptionText: true,
               onItemSelect: function () {
                   var $elx = $(this);
                   var $eli = $elx.find('a');
                   var oid = $eli.data('id');
                   var name = $eli.data('name');
                   if (!_.isUndefined(oid)) {
                       if ($elx.hasClass('active')) {
                           self.m_mediaselection[oid] =
                           {
                               ResourceId: oid,
                               Name: name,
                               Length: 100 /* hardcode image length = 4 */
                           };

                       }
                       else {
                           delete self.m_mediaselection[oid];
                       }


                       self.buildSelected();


                   }


               },

           });

            $('a.mcs-open').trigger('click');
        },
        buildSelected: function ()
        {
            var self = this;
            var $selcontainer = this.$el.find('.selected_items');
            var list = $selcontainer.find('ul.news-list');
            list.empty();
            var index = 0;
            _.each(self.m_mediaselection, function (e, o) {
                index++;
                var snippet = '<li><div class="icon pull-left">' + index + '</div><div class="news-item-info" style="padding:5px 5px;"> <h5>' + e.Name + '</h5> </div></li>';
                list.append(snippet);
            });
        },
        render: function ()
        {
            var self = this;
             
            var $feed = self.$el.find('.mediainfo');
            $feed.slimscroll({ height: 'auto', size: '5px', alwaysVisible: false, railVisible: true }); 
            self._buildMediaCategory();
          
        },
        _buildMediaCategory: function () {
            var self = this;
            var wrapper = self.$el.find('.wrapper');
            _.each(self.model.Media, function (o, e) {
                var snippet = _.template($('#media_category_template').html(), {
                    mediacategory_id: e, category_item: o
                });
                wrapper.append(snippet);

            });
        }

    });

    $(function () {

        var mainView = new CampaignsMediaSelector({
            el: '#media_selector_view',
            model: __jsonModel
        });

        mainView.render();

    });

</script>


<script id="media_category_template" type="text/html">
    <section class="feed-item" style="cursor:pointer;">
        <div class="icon pull-left"></div>
        <div class="feed-item-body">
            <div class="text">
                <div class="text-justify">
                    <a id="<@= mediacategory_id @>" data-action="select_category"><@= category_item.Name @></a>
                </div>
                <span class="pull-right" style="margin-right: 10px;">
                    <span class="badge badge-success badge-sm"><@=category_item.Count @></span>
                </span>
            </div>
        </div>
    </section>
</script>

<script id="image_resources_template" type="text/html">
    <div id="image_select">
        <select name="media">
            <@ _.each(mediaresources,function(o,e) { @>
            <option value="<@= e @>" data-name="<@= o.Name @>" data-image="<@= o.Thumbnail @>"><@=o.Name @></option>
            <@ }); @>   
        </select>
    </div>
</script>
<script id="html_resources_template" type="text/html">
    <div id="html_select">
        <select name="media">
            <@ _.each(mediaresources,function(o,e) { @>
            <option value="<@= e @>" data-name="<@= o.Name @>" data-image="<@= o.Thumbnail @>"><@=o.Name @></option>
            <@ }); @>
        </select>
    </div>
</script>

