<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCTestjs01_1.ascx.cs" Inherits="HIS.Web.UI.Site.ZScript.Controls.UCTestjs01_1" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">
            TEST

             <button id="click01" type="button" class="btn btn-success">Save</button>
            <div id="campaign_editor" class="tab-content">
                <div id="layout_selector_view" class="tab-pane fade in">
                    A
                </div>
                <div id="content_editor_view" class="tab-pane active fade in">
                    <div class="row">
                        <div class="col-md-9">
                            <div id="sequencerview" class="tab-pane active fade in">
                                <div id="headerview" class="row">
                                    <div class="col-md-12">
                                        <div class="btn-groups">
                                            <button type="button" data-action="add_timeline" data-target="#layout_selector_view" class="btn btn-primary"><i class="fa fa-plus"></i>&nbsp;Add Timeline</button>
                                            <button type="button" id="removeTimelineBtn" class="btn btn-primary"><i class="fa fa-minus"></i>&nbsp;Remove Timeline</button>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div id="timeline_board" class="col-md-12">
                                    </div>
                                    <div id="timeline_board02" class="col-md-12">
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div id="storyline_board" class="col-md-12">
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div id="content_board" class="col-md-12">
                                        <div class="well well-sm" style="min-height: 50px;">
                                            <div class="row">
                                                <div class="col-md-10">
                                                    <div class="btn-groups">
                                                        <div class="btn-group">
                                                            <button type="button" class="btn btn-primary"><i class="fa fa-plus"></i>&nbsp;  Add Content &nbsp;</button>
                                                            <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown">
                                                                <span class="caret"></span>
                                                                <span class="sr-only">Toggle Dropdown</span>
                                                            </button>
                                                            <ul class="dropdown-menu" role="menu">
                                                                <li><a data-action="select_image" href="#">Image</a></li>
                                                                <li><a data-action="select_video" href="#">Video</a></li>
                                                                <li><a href="#">...</a></li>
                                                                <li class="divider"></li>
                                                                <li><a href="#">...</a></li>
                                                            </ul>
                                                        </div>
                                                        <button id="removeBlockButton" type="button" class="btn btn-info">
                                                            <i class="fa fa-minus"></i>&nbsp; Remove Content
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel" style="min-height: 400px;">
                                <div id="blockprops_board" class="col-md-12">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="content_selector_view" class="tab-pane fade in">
                    <div id="blockselector_board" class="panel" style="min-height: 200px;">
                        <div class="tab-content">
                            <div id="tab_image" class="tab-pane fade in">
                                <button type="button" class="btn btn-info btn-block return-selectcontrol" data-toggle="tab" data-target="#selectcontrol">Return </button>
                                <div id="image_blocks_container"></div>
                            </div>
                            <div id="tab_video" class="tab-pane fade in">
                                <button type="button" class="btn btn-info btn-block return-selectcontrol" data-toggle="tab" data-target="#selectcontrol">Return </button>
                                <div id="video_blocks_container"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</div>

<script type="text/javascript">


    var TestJS01View = Backbone.View.extend(
    {
        initialize: function (options) {
            var self = this;
            console.log("**************************************TestJS01View:initialize:self.model************************************************");
            console.log(self.model);
            this.options = options;
            self.m_campaigndata = self.model;
            self.m_screendata = self.model.ScreenData;
            self.m_campaign_id = self.model.CampaignId;
            self.m_screentemplate = self.model.ScreenTemplate;
            self.sequencerView = new MySequencerView({ el: '#timeline_board', editor: self });
            self.storylineView = new MyStorylineView({ el: '#storyline_board', editor: self, sequencer: self.sequencerView });


        },
        _render: function () {
            var self = this;
            self._loadTimelinesFromDB();
            self.sequencerView.selectFirstTimeline();
            var storyline = self.storylineView;
            storyline._render();
        },
        _loadTimelinesFromDB: function () {
            var self = this;

            _.each(self.m_screendata, function (o, i) {
                self.sequencerView.createTimelineThumbnailUI({
                    timelineID: o.TimelineId,
                    orientation: o.Orientation,
                    resolution: o.Resolution,
                    screentype: o.ScreenType,
                    viewerChannels: o.ViewerChannels,
                    channelBlocks: o.ChannelBlocks,
                    blockProperties: o.BlockProperties,

                });
            });
        },

    });


    var MySequencerView = Backbone.View.extend({
        initialize: function (options) {
            var self = this;
            this.options = options;
            self.m_thumbsContainer = this.$el;
            self.m_timelines = {};
            self.m_screenTemplates = {};
            self.m_blockproperties = {};
            self.m_model = {};
            self.m_editor = self.options.editor;

        },
        selectFirstTimeline: function () {

        },
        createTimelineThumbnailUI: function (i_screenProps) {
            var self = this;
            var index = -1;
            var elem = undefined;
            var campaign_timeline_id = i_screenProps.timelineID;

            var props = ScreenTemplate[i_screenProps.orientation][i_screenProps.resolution][i_screenProps.screentype];
            console.log("***************************MySequencerView:createTimelineThumbnailUI: ScreenTemplate************************************************");
            console.log(ScreenTemplate);
            console.log("**************************************MySequencerView:createTimelineThumbnailUI: props************************************************");
            console.log(props);
            var blockProperties = i_screenProps.blockProperties;



            self.m_model[campaign_timeline_id] = {
                channelblocks: i_screenProps.channelBlocks,
                viewerchannels: i_screenProps.viewerChannels,
            };

            _.each(props, function (o, i) {
                console.log("loop For Extend Properties");
                o.campaign_timeline_id = campaign_timeline_id;
                o.timeline_channel_id = i_screenProps.viewerChannels[o.id];
                o.campaign_timeline_board_viewer_id = o.id;
            });

            _.each(blockProperties, function (properties, block) {
                self.m_blockproperties[block] = properties;
            });

            var screenTemplateData = {
                orientation: i_screenProps.orientation,
                resolution: i_screenProps.resolution,
                screenProps: props,
                scale: '14'
            };

            var screenTemplatess = new MyScreenTemplateFactory({
                i_screenTemplateData: screenTemplateData,
                i_type: Backbone.CONSTS.ENTIRE_SELECTABLE,
                i_owner: this
            });


            var snippet = screenTemplatess.create();
            var elementID = $(snippet).attr('id');

            self.m_timelines[campaign_timeline_id] = elementID;
            self.m_screenTemplates[campaign_timeline_id] = screenTemplatess;


            switch (index) {

                case -1: {

                    self.m_thumbsContainer.append(snippet);
                    $("#timeline_board02").append("name");
                    screenTemplatess.frameSelectable();
                    break;
                }

            }





        },
        getTimelineDuration: function (timeline_id) {

            var self = this;
            console.log("inside getTimelineDuration");
            console.log(self.m_model);
            console.log(timeline_id);
            var channelblocks = self.m_model[timeline_id].channelblocks;
            var duration = 0;
            _.each(channelblocks, function (blocks, e) {
                var sumlength = 0;
                _.each(blocks, function (x, y) {
                    sumlength += x.Length;
                });
                if (sumlength >= duration)
                    duration = sumlength;
            });

            return duration;
        },
    });







    var MyStorylineView = Backbone.View.extend(
    {
        initialize: function (options) {

            this.options = options;

            var self = this;

            self.m_editor = self.options.editor;

            self.m_sequencer = self.options.sequencer;

            self.m_timelineChannels = {};

            self.m_storyWidth = 0;

            self.m_owner = self;

            self.m_selectedTimelineID = undefined;

            self.m_selectedChannel = undefined;

            self.m_screenTemplate = undefined;

            self.m_storyBoardTemplate = $('#storyline_template').html();



            /*self.m_blocksel = new BlockSelectorView({ el: '#blockselector_board' });*/



            self._updateWidth();
            self._listenToEvents();

        },

        _render: function (callback) {
            var self = this;
            /*call by TestJS01View:render*/
            self.m_callback = callback;

            if (_.isUndefined(self.m_render)) {

                self.m_render = _.debounce(function () {
                    $(self.el).empty();

                    self.m_storylineContainerSnippet = $(self.m_storyBoardTemplate).find(Elements.STORYLINE_CONTAINER).parent();

                    self.m_TableSnippet = $(self.m_storyBoardTemplate).find('table').parent();

                    self.m_ChannelSnippet = $(self.m_storyBoardTemplate).find(Elements.CLASS_STORYLINE_CHANNEL).parent();

                    self._populateScala();

                    self._populateChannels();

                    self._listenSelections();

                    self._renderCallback();


                }, 300);
            }
            self.m_render();


        },

        _populateScala: function () {



            var self = this;
            var sq = self.m_sequencer;
            var timeline_id = self.m_selectedTimelineID;


            var ticks = [];

            var format = 's';

            var totalDuration = sq.getTimelineDuration(timeline_id);

            if (totalDuration > 420) {
                totalDuration = totalDuration / 60;
                format = 'm';
            }

            var tick = totalDuration / 4;

            for (i = 1; i < 5; i++) {
                tick = parseFloatToDouble(tick);
                ticks.push(tick * i);
            }

            ticks.unshift(0);
            ticks[ticks.length - 1] = totalDuration;

            var l = String((ticks[ticks.length - 1]).toFixed(2)).length;
            var lastTick = '';
            var scalaRuler = $(self.m_TableSnippet).find(Elements.CLASS_SCALA_RULER);

            for (i = 0; i < ticks.length; i++) {
                if (i == ticks.length - 1) {
                    lastTick = 'width="1%"';
                }
                var value = padZeros(parseFloatToDouble(ticks[i]), l) + format;
                $(scalaRuler).append('<td class="scalaNum"' + lastTick + '>' + value + '</td>');
            }

            $(self.el).append(self.m_TableSnippet);


        },
        _populateChannels: function () {
            var self = this;
            var tpl = self.m_screenTemplate;

            var n = 0;
            _.each(tpl.m_screenProps, function (props, key) {
                var timeline_channel_id = props.timeline_channel_id;
                var viewer_id = props.id;
                var channelSnippet = _.template(_.unescape(self.m_ChannelSnippet.html()), { value: n + 1 });
                $(self.m_storylineContainerSnippet).find('section').append(channelSnippet);
                var channelHead = $(self.m_storylineContainerSnippet).find(Elements.CLASS_CHANNEL_HEAD + ':last');
                var channelBody = $(self.m_storylineContainerSnippet).find(Elements.CLASS_CHANNEL_BODY + ':last');
                $(channelHead).attr('data-timeline_channel_id', timeline_channel_id);
                $(channelBody).attr('data-timeline_channel_id', timeline_channel_id);
                $(channelHead).attr('data-campaign_timeline_board_viewer_id', viewer_id);
                $(channelBody).attr('data-campaign_timeline_board_viewer_id', viewer_id);
                self._populateBlocks(timeline_channel_id, channelBody);
                n++;
            });


            $(self.el).append(self.m_storylineContainerSnippet);

        },
        _populateBlocks: function (channel_id, channel_body) {
            var self = this;
            var sq = self.m_sequencer;
            var timeline_id = self.m_selectedTimelineID;
            var blocks = sq.getChannelBlocks(timeline_id, channel_id);

            var totalDuration = sq.getTimelineDuration(timeline_id);

            _.each(blocks, function (o, i) {

                var bWidth = (o.Length / totalDuration) * 100;

                var blockSnippet = _.template($('#iblock').html(), {
                    blockwidth: 'width:' + bWidth + '%;', block_id: i,
                });
                $(channel_body).append(blockSnippet);

            });


        },
        _updateWidth: function () {
            var self = this;
            self.m_storyWidth = parseInt($(Elements.STORYLINE_CONTAINER).width()) - 25;
            $(Elements.CLASS_CHANNEL_BODY_CONTAINER).width(self.m_storyWidth);
        },
        _listenToEvents: function () {
            var self = this;
            amplify.subscribe(Backbone.EVENTS.APP_SIZED, function (e) {
                self._updateWidth();
                self._render();

            });

            amplify.subscribe(Backbone.EVENTS.ON_BLOCK_UPDATED, function (e) {
                self._render({ lastblock: e.blockID });
            });

            amplify.subscribe(Backbone.EVENTS.ON_TIMELINE_SELECTED, function (e) {

                self.m_selectedTimelineID = e.timelineID;
                self.m_screenTemplate = e.screenTemplate;
                self._removeChannelSelection(); /* remove channel selections */
                self.m_blockprops.removeBlockProperties();
                self._render();

            });

        },

    });


    $(function () {


        $('#click01').on('click', function (e) {

            __JsonModel(<%= this.JsonData %>);


            var view = new TestJS01View(
            {
                model: __jsonModel,
                el: '#campaign_editor'
            });

            view._render();

        });



    });

</script>

<script id="storyline_template" type="text/html">
    <div>
        <table width="100%">
            <tr class="scalaRuler"></tr>
            <tr>
                <td class="scalaDiv">| </td>
                <td class="scalaDiv">|  </td>
                <td class="scalaDiv">| </td>
                <td class="scalaDiv">| </td>
                <td class="scalaDiv">| </td>
            </tr>
        </table>
    </div>
    <div>
        <div id="storylineContainer">
            <section />
            <div style="clear: both; height: 1px"></div>
        </div>
    </div>
    <div>
        <div class="storylineChannel">
            <div class="channelHead" style="text-align: center">
                <@= value @>       
            </div>
            <div class="channelBodyContainer" style="width: 95%">
                <div class="channelBody">
                </div>
            </div>
        </div>
    </div>
    <div>
        <div class="timelineBlock">
            <span class="crop"><@= label @></span>
        </div>
    </div>
</script>
