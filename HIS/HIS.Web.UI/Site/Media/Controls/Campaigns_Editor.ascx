<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Campaigns_Editor.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Campaigns_Editor" %>
<div class="row">
    <div class="col-md-12">
        <section class="widget">
            <header>
                <fieldset>
                    <legend>Campaign Editor</legend>
                </fieldset>
            </header>
            <div id="campaign_editor" class="tab-content">
                <div id="layout_selector_view" class="tab-pane fade in">
                </div>
                <div id="content_editor_view" class="tab-pane active fade in">
                    <div class="row">
                        <div class="col-md-9">
                            <div id="sequencerview" class="tab-pane active fade in">
                                <div id="headerview" class="row">
                                    <div class="col-md-10">
                                        <div class="btn-groups">
                                            <button type="button" data-action="add_timeline" class="btn btn-primary"><i class="fa fa-plus"></i>&nbsp;Add Timeline</button>
                                            <button type="button" data-action="remove_timeline" class="btn btn-primary"><i class="fa fa-minus"></i>&nbsp;Remove Timeline</button>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="pull-right">
                                            <button type="button" data-action="return_to_list" class="btn btn-primary"><i class="fa fa-arrow-left"></i>&nbsp;Return</button>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div id="timeline_board" class="space horizontal-images">
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
                                                <div class="col-md-6">
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
                                                                <li><a data-action="select_stream" href="#">Stream</a></li>
                                                                <li><a data-action="select_html" href="#">HTML</a></li>
                                                                <li class="divider"></li>
                                                                
                                                            </ul>
                                                        </div>
                                                        <button id="removeBlockButton" type="button" class="btn btn-info">
                                                            <i class="fa fa-minus"></i>&nbsp; Remove Content
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="channel_blocks_info">
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

<div class="modal fade" id="mModal" tabindex="-1" role="dialog" aria-labelledby="mModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                <h4 class="modal-title" id="mModalLabel">Error!
                </h4>
            </div>
            <div class="modal-body">
                Please Select Channel to Add!
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-inverse" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>
<div id="modal_container">
</div>

<script type="text/javascript">

    var CampaignsEditorView = Backbone.View.extend(
    {
        initialize: function (options) {

            var self = this;
            this.options = options;
            self.m_campaigndata = self.model;
            self.m_screendata = self.model.ScreenData;
            self.m_campaign_id = self.model.CampaignId;
            self.m_screentemplate = self.model.ScreenTemplate;
            self.m_selected_timeline_id = -1;
            self.sequencerView = new SequencerView({ el: '#timeline_board', editor: self });

            self.blockpropsView = new BlockPropertiesView(
            {
                el: '#blockprops_board',
                editor: self, sequencer: self.sequencerView
            });

            self.storylineView = new StorylineView({ el: '#storyline_board', editor: self, sequencer: self.sequencerView, blockprops: self.blockpropsView });



            self.blockselectorView = new BlockSelectorView(
            {
                el: '#blockselector_board', editor: self
            });

            self._listenToEvents();
        },
        events:
        {
            'show.bs.tab button[data-toggle="tab"]': '_viewchanged',
            'click a[data-action=select_image]': '_imageSelection',
            'click a[data-action=select_video]': '_videoSelection',
            'click a[data-action=select_stream]': '_streamSelection',
            'click a[data-action=select_html]': '_htmlSelection',             
            'click button[data-action=add_timeline]': '_addTimeline',
            'click button[data-action=remove_timeline]': '_removeTimeline',
            'click #removeBlockButton': 'removeBlock',
        },
        _streamSelection: function (e) {
            var self = this;
        },
        _videoSelection: function (e) {
            var self = this;
        },
        _htmlSelection: function (e) {
            var self = this;
            var storyline = self.storylineView;
            var channel = storyline.m_selectedChannel;

            if (_.isUndefined(channel)) {

                $('#mModal').modal({
                    backdrop: 'static'
                });
            }
            else {

                var oJson =
                {
                    Action: 'SelectMedia',
                    Data: {
                        Context: 'HTML5',
                        DateTimeRequest: moment().format(),
                        Channel: storyline.m_selectedChannel,
                    }
                };
                _Post(document.URL, oJson);
            }
        },
        _imageSelection: function (e) {
            var self = this;
            var storyline = self.storylineView;
            var channel = storyline.m_selectedChannel;

            if (_.isUndefined(channel)) {
                $('#mModal').modal({ backdrop: 'static' });

            } else {

                var oJson =
                {
                    Action: 'SelectMedia',
                    Data: {
                        Context: 'Image',
                        DateTimeRequest: moment().format(),
                        Channel: storyline.m_selectedChannel,
                    }

                };
                _Post(document.URL, oJson);
            }

        }, 
        _addTimeline: function (e) {

            var self = this;
            var oJson = {
                Action: 'SelectLayout',
                Data: {

                    DateTimeRequest: moment().format(),
                    Campaign: self.m_campaign_id,
                    ScreenTemplate: self.m_screentemplate
                }

            };
            _Post(document.URL, oJson);

        },
        _removeTimeline: function (e) {
            var self = this;


            var container = $('#modal_container');
            container.empty();

            var snippet = _.template($('#modal_confirm_template').html(),
            {
                Question: 'Remove Timeline ?',

            });

            container.append(snippet);

            container.find('button[data-action=Yes]').on('click', function (e) {

                amplify.request(Backbone.REQUEST.REMOVE_CAMPAIGNS_TIMELINE,
                JSON.stringify({
                    Campaign: self.model.CampaignId,
                    Timeline: self.storylineView.m_selectedTimelineID

                }), function (data) {
                    /* rebind */
                    container.find('.modal').modal('hide');

                    var oJson = {};
                    oJson.Action = 'EditCampaign';
                    oJson.DateTimeRequest = moment().format();
                    oJson.DataTarget = data.Campaign;
                    _Post(document.URL, oJson);

                });
            });

            container.find('.modal').modal('show');

            /*
            
            */


        },
        addBlock: function () {
            var self = this;
            var storyline = self.storylineView;
            var sequencer = self.sequencerView;
            var channel = storyline.m_selectedChannel;
            var timeline = storyline.m_selectedTimelineID;

            if (_.isUndefined(channel)) {
                $('#mModal').modal({ backdrop: 'static' });
            } else {

                sequencer.addChannelBlock(timeline, channel, 'E8D49E8B-1235-E411-9898-954E51CDDD96', 10);

            }
        },
        removeBlock: function () {
            var self = this;
            var storyline = self.storylineView;
            var sequencer = self.sequencerView;
            var channel = storyline.m_selectedChannel;
            var timeline = storyline.m_selectedTimelineID;
            var selectedblock = storyline.selected_block_id;

            if (_.isUndefined(channel)) {
                $('#mModal').modal({
                    backdrop: 'static'
                });

            } else {

                if (_.isUndefined(channel)) {
                    $('#mModal').modal({
                        backdrop: 'static'
                    });
                } else {

                    sequencer.removeChannelBlock(timeline, channel, storyline.selected_block_id);
                    storyline._render();
                }
            }
        },
        _testUI: function () {
            var self = this;
            self._changeView('#layout_selector_view');
        },
        /* to change main tab! */
        _viewchanged: function (e) {
            var self = this;
            var target = $(e.target).data('target');


            switch (target) {
                case '#layout_selector_view':
                    break;
                case '#content_selector_view':
                    self.blockselectorView._render();
                    break;

            }
        },
        _changeView: function (target) {
            var self = this;
            var element = this.$el.find('button[data-target="' + target + '"]');

        },
        _render: function () {
            var self = this;
            self._loadTimelinesFromDB();
            var timeline = self.model.Timeline;
            var position = '';

            if (null != timeline) {
                position = self.sequencerView._selectTimeline(timeline);
            }
            else {
                self.sequencerView.selectFirstTimeline();
            }

            $('#timeline_board').mCustomScrollbar
            ({
                axis: 'x',
                theme: 'dark-3',
                advanced: {
                    autoExpandHorizontalScroll: true
                }
            });

            $('#timeline_board').mCustomScrollbar('scrollTo', position);
        },
        _loadTimelinesFromDB: function () {
            var self = this;

            _.each(self.m_screendata, function (o, i) {
                self.sequencerView.createTimelineThumbnailUI({
                    timelineID: o.TimelineId, orientation: o.Orientation, resolution: o.Resolution, screentype: o.ScreenType,
                    viewerChannels: o.ViewerChannels,
                    channelBlocks: o.ChannelBlocks,
                    blockProperties: o.BlockProperties,
                });
            });
        },
        _listenToEvents: function () {
            var self = this;
            amplify.subscribe(Backbone.EVENTS.STORYLINE_RENDER_COMPLETED, function (e) {
                if (!_.isUndefined(e.lastblock)) {
                    $lastblock = $("#" + e.lastblock);
                    $parent = $lastblock.parent();
                }
            });
        }
    });

    /*
    ------------ Sequencer view--------------
    
    */

    var SequencerView = Backbone.View.extend({

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
            var self = this;
            for (var i in self.m_timelines) {
                self._selectTimeline(i);
                break;
            }
        },
        _selectTimeline: function (timelineID) {
            var self = this;
            var total = $(self.m_thumbsContainer).find('[data-campaign_timeline_id="' + timelineID + '"]').eq(0).trigger('click');
            if (total.length == 0) {
                return -1;
            }

            /*var elo = $(self.m_thumbsContainer).find('[data-timeline-id="' + timelineID + '"]');*/
            var position = '#' + total.parents('svg').prop('id');

            return position;
        },
        createTimelineThumbnailUI: function (i_screenProps) {

            var self = this;
            var index = -1;
            var elem = undefined;
            var campaign_timeline_id = i_screenProps.timelineID;
            var props = ScreenTemplate[i_screenProps.orientation][i_screenProps.resolution][i_screenProps.screentype];
            var blockProperties = i_screenProps.blockProperties;

            self.m_model[campaign_timeline_id] = {
                channelblocks: i_screenProps.channelBlocks,
                viewerchannels: i_screenProps.viewerChannels,
            };

            var _props = {};



            _.each(props, function (o, i) {
                _props[i] = _.clone(o);
                _props[i].campaign_timeline_id = campaign_timeline_id;
                _props[i].timeline_channel_id = i_screenProps.viewerChannels[o.id];
                _props[i].campaign_timeline_board_viewer_id = o.id;
            });



            _.each(blockProperties, function (properties, block) {
                self.m_blockproperties[block] = properties;
            });

            var screenTemplateData =
            {
                orientation: i_screenProps.orientation,
                resolution: i_screenProps.resolution,
                screenProps: _props,
                scale: '14'
            };

            var screenTemplate = new ScreenTemplateFactory({
                i_screenTemplateData: screenTemplateData,
                i_type: Backbone.CONSTS.ENTIRE_SELECTABLE,
                i_owner: this
            });


            var snippet = screenTemplate.create();




            var elementID = $(snippet).attr('id');

            self.m_timelines[campaign_timeline_id] = elementID;
            self.m_screenTemplates[campaign_timeline_id] = screenTemplate;
            screenTemplate.selectablelDivision();
            screenTemplate.activate();

            switch (index) {
                case -1: {
                    self.m_thumbsContainer.append(snippet);
                    screenTemplate.frameSelectable();
                    break;
                }
                case 0: {
                    elem = self.m_thumbsContainer.children().eq(0);
                    if (elem.length > 0) {
                        $(snippet).insertBefore(elem);
                    } else {
                        self.m_thumbsContainer.append(snippet);
                    }
                    screenTemplate.frameSelectable();
                    break;
                }

                default: {
                    elem = self.m_thumbsContainer.children().eq(index - 1);
                    $(snippet).insertAfter(elem);
                    screenTemplate.frameSelectable();
                    break;
                }
            }




        },
        selectViewer: function (timeline_id, board_viewer_id) {
            var self = this;
            self.m_screenTemplates[timeline_id].selectDivison(board_viewer_id);

        },
        getScreenTemplate: function (timeline_id) {
            var self = this;
            return self.m_screenTemplates[timeline_id];
        },
        getChannelBlocks: function (timeline_id, channel_id) {
            var self = this;
            
            return self.m_model[timeline_id].channelblocks[channel_id];
        },
        getTimelineDuration: function (timeline_id) {
            var self = this;
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
        getBlockProperties: function (block_id) {
            var self = this;
            return self.m_blockproperties[block_id];
        },
        editChannelBlock: function (obj) {
            var self = this;

            var timeline_id = obj.timeline, channel_id = obj.channel, block_id = obj.block, timespan = obj.timespan;
            /* request channelblockedit  success ! error */
            amplify.request(Backbone.REQUEST.EDIT_CHANNEL_BLOCK, JSON.stringify({ blockID: block_id, timespan: timespan }),
                function (data, status) {
                    var block_length = data.block_length;
                    self.m_model[timeline_id].channelblocks[channel_id][block_id].Length = block_length;
                    self.m_blockproperties[block_id].Length = block_length;
                    amplify.publish(Backbone.EVENTS.ON_BLOCK_UPDATED, { blockID: block_id })
                });



            return false;

        },
        addChannelBlock: function (timeline_id, channel_id, resource_id, block_length) {

            var self = this;
            var uniqueid = getUUID();

            amplify.request(Backbone.REQUEST.ADD_CHANNEL_BLOCK,
                JSON.stringify(
            {
                channelID: channel_id,
                resourceID: resource_id,
                blockLength: block_length

            }), function (data) {
                /* check success & fail */
                self.m_model[timeline_id].channelblocks[channel_id][data.block_id] = {
                    ResourceType: 2,
                    Length: 150,
                };

                /* blockproperties might not be needed */
                self.m_blockproperties[data.block_id] = {
                    Type: 2,
                    Length: 150,
                };

                amplify.publish(Backbone.EVENTS.ON_BLOCK_UPDATED,
                {
                    blockID: data.block_id
                })

            });



            return uniqueid;
        },
        removeChannelBlock: function (timeline_id, channel_id, block_id) {
            var self = this;
            delete self.m_model[timeline_id].channelblocks[channel_id][block_id];
            delete self.m_blockproperties[block_id];
        }

    });


    /*
       -------------------------STORYLINE---------------------------------
    */
    var StorylineView = Backbone.View.extend(
    {
        initialize: function (options) {

            this.options = options;

            var self = this;

            self.m_editor = self.options.editor;

            self.m_sequencer = self.options.sequencer;

            self.m_blockprops = self.options.blockprops;

            self.m_timelineChannels = {};

            self.m_storyWidth = 0;

            self.m_owner = self;

            self.m_selectedTimelineID = undefined;

            self.m_selectedChannel = undefined;

            self.m_screenTemplate = undefined;

            self.m_storyBoardTemplate = $('#storyline_template').html();

            self.m_blocksel = new BlockSelectorView({ el: '#blockselector_board' });

            /* channel blocks info */
            self.m_channel_blocks_info = '.channel_blocks_info';
            self.m_channel_blocks_info_template = '#channel_blocks_info_template';
            self.m_channel_blocks_container = '#channel_blocks_container';

            self._updateWidth();

            /* listen to events */
            self._listenToEvents();
        },

        _render: function (callback) {
            var self = this;

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
        _renderCallback: function () {
            var self = this;
            if (!_.isUndefined(self.m_callback)) {
                var callback = self.m_callback;
                if (!_.isUndefined(callback.lastblock)) {

                    var target = $(ident(callback.lastblock));
                    target.parent().triggerHandler('click');
                }
            }

        },
        _populateScala: function () {

            var self = this;
            var sq = self.m_sequencer;
            var timeline_id = self.m_selectedTimelineID;
            console.log("populate scala");
            console.log(timeline_id);

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
                /* var value = padZeros(parseFloatToDouble(ticks[i]), l) + format; */
                var value = parseFloatToDouble(ticks[i]) + format;
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
            console.log("**************************************DEBUG BY AZRUL ***********************************");
            console.log(self.m_storylineContainerSnippet);
            console.log("*****************************************************************************************");
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

        _listenSelections: function () {
            var self = this;
            var ch = $(Elements.CLASS_CHANNEL_HEAD);
            var sch = $(Elements.CLASS_STORYLINE_CHANNEL);
            var tbl = $(Elements.CLASS_TIMELINE_BLOCK);

            ch.off('click').on('click', function (e) {
                $.proxy(self._blockChannelSelected(e), self);
                amplify.publish(Backbone.EVENTS.CAMPAIGN_TIMELINE_CHANNEL_SELECTED, {
                    owner: this,
                    selectedChannel: self.m_selectedChannel
                });
            });

            sch.off('click').on('click', function (e) {
                $.proxy(self._blockChannelSelected(e), self);
                amplify.publish(Backbone.EVENTS.CAMPAIGN_TIMELINE_CHANNEL_SELECTED, {
                    owner: this,
                    selectedChannel: self.m_selectedChannel
                });

            });

            tbl.off('click').on('click', function (e) {
                $.proxy(self._blockSelected(e), self);
            });

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
        _blockSelected: function (e) {

            var self = this;

            e.stopImmediatePropagation();

            var blockElem = $(e.target).parent();

            var selected_block_id = $(blockElem).data('timeline_channel_block_id');

            if (_.isUndefined(selected_block_id))
                blockElem = $(e.target);

            self.selected_block_id = $(blockElem).data('timeline_channel_block_id');

            e.target = blockElem[0];

            self._blockChannelSelected(e);

            $(blockElem).addClass(unclass(Elements.CLASS_TIMELINE_BLOCK_SELECTED));
            $(blockElem).find('a').addClass('active');
            $(self.m_channel_blocks_container).find('li.' + self.selected_block_id).addClass('active');

            amplify.publish(Backbone.EVENTS.STORYLINE_BLOCK_SELECTED, {
                timeline: self.m_selectedTimelineID,
                channel: self.m_selectedChannel,
                block: self.selected_block_id
            });

            return false;
        },
        _blockChannelSelected: function (e) {
            var self = this;

            var chHead;
            e.stopImmediatePropagation();
            var blockElem = $(e.target);
            if (_.isUndefined($(blockElem).data('timeline_channel_id')))
                blockElem = $(blockElem).parent();
            if (_.isUndefined($(blockElem).attr('class')))
                return false;
            if ($(blockElem).hasClass(unclass(Elements.CLASS_STORYLINE_CHANNEL)))
                blockElem = $(blockElem).find(Elements.CLASS_CHANNEL_HEAD);
            var timeline_channel_id = $(blockElem).data('timeline_channel_id');
            var campaign_timeline_board_viewer_id = $(blockElem).data('campaign_timeline_board_viewer_id');
            self.m_selectedChannel = timeline_channel_id;
            var screenData = {
                m_owner: self,
                campaign_timeline_id: self.m_selectedTimelineID,
                campaign_timeline_board_viewer_id: campaign_timeline_board_viewer_id
            };

            self._removeBlockSelection();
            self._addChannelSelection(self.m_selectedChannel);

            self.m_sequencer.selectViewer(screenData.campaign_timeline_id, screenData.campaign_timeline_board_viewer_id);
            /*
            amplify.publish(Backbone.EVENTS.ON_VIEWER_SELECTED, screenData);
            */


            return false;
        },
        _addChannelSelection: function (i_selectedChannel) {
            var self = this;
            if (_.isUndefined(i_selectedChannel))
                return;
            self._removeChannelSelection();
            self.m_selectedChannel = i_selectedChannel;
            var blockElem = $(Elements.STORYLINE_CONTAINER).find('[data-timeline_channel_id="' + i_selectedChannel + '"]');
            $(blockElem).addClass(unclass(Elements.CLASS_CHANNEL_HEAD_SELECTED));
            self._addChannelBlocksInfo(i_selectedChannel);

        },
        _addChannelBlocksInfo: function (i_selectedChannel) {
            var self = this;
            if (_.isUndefined(i_selectedChannel))
                return;
            var sq = self.m_sequencer;
            var timeline_id = self.m_selectedTimelineID;
            var blocks = sq.getChannelBlocks(timeline_id, i_selectedChannel);


            var cbinfo = $(self.m_channel_blocks_info);
            cbinfo.empty();

            var count = _.keys(blocks).length;

            if (count > 0) {
                var snippet = _.template($(self.m_channel_blocks_info_template).html(), { blocksinfo: blocks });
                cbinfo.append(snippet);
            }

        },
        _removeChannelSelection: function () {
            var self = this;
            self.m_selectedBlockID = undefined;
            self.m_selectedChannel = undefined;
            $(Elements.CLASS_CHANNEL_HEAD_SELECTED).removeClass(unclass(Elements.CLASS_CHANNEL_HEAD_SELECTED));
            self._removeChannelBlocksInfo();
        },
        _removeChannelBlocksInfo: function () {
            var self = this;
            var cbinfo = $(self.m_channel_blocks_info);
            cbinfo.empty();

        },
        _removeBlockSelection: function () {
            var self = this;
            self.m_selectedBlockID = undefined;
            $(Elements.CLASS_TIMELINE_BLOCK_SELECTED).find('a').removeClass('active');
            $(Elements.CLASS_TIMELINE_BLOCK, Elements.STORYLINE_CONTAINER).removeClass(unclass(Elements.CLASS_TIMELINE_BLOCK_SELECTED));
        },
        _updateWidth: function () {
            var self = this;
            self.m_storyWidth = parseInt($(Elements.STORYLINE_CONTAINER).width()) - 25;
            $(Elements.CLASS_CHANNEL_BODY_CONTAINER).width(self.m_storyWidth);
        },



    });

    var BlockPropertiesView = Backbone.View.extend(
    {
        initialize: function (options) {
            var self = this;
            self.el_id = '#blockprops_board';
            self.options = options;
            self.m_editor = self.options.editor;
            self.m_sequencer = self.options.sequencer;
            self.m_selected_timeline_id = undefined;
            self.m_selected_block_id = undefined;
            self.m_selected_channel_id = undefined;

            self._listenSelections();
            self._listenToEvents();

        },
        _render: function () {
            var self = this;
            var el0 = $(self.el_id);
            el0.empty();
            var sq = self.m_sequencer;
            var block = sq.getBlockProperties(self.m_selected_block_id);

            if (_.isUndefined(block))
                return;

            var sFormat = StringFormattedTimeSpan(block.Length);
            var timeSpan = SecondsToTimeSpan(sFormat);
            var snippet = _.template($('#blockprops_template').html(),
            {
                timespan: sFormat
            });

            el0.append(snippet); /* append first to get dom el */

            var hp = el0.find('input.h_placeholder');
            var mp = el0.find('input.m_placeholder');
            var sp = el0.find('input.s_placeholder');
            var timelabel = el0.find('span.timelabel');

            $(hp).ionRangeSlider(
            {
                min: 0, max: 4, from: timeSpan.hours, postfix: ' hours', hasGrid: true, gridMargin: 1,
                onChange: function (e) {
                    self._editBlockProperties('input.h_placeholder', timelabel, timeSpan.hours);
                }
            });
            $(mp).ionRangeSlider({
                min: 0, max: 59, from: timeSpan.minutes, postfix: ' minutes', hasGrid: true, gridMargin: 5,
                onChange: function (e) {
                    self._editBlockProperties('input.m_placeholder', timelabel, timeSpan.minutes);
                }
            });

            $(sp).ionRangeSlider({
                min: 0, max: 59, from: timeSpan.seconds, postfix: ' seconds', hasGrid: true, gridMargin: 10,
                onChange: function (e) {
                    self._editBlockProperties('input.s_placeholder', timelabel, timeSpan.seconds);
                }
            });



        },
        _listenSelections: function () {
            var self = this;
            amplify.subscribe(Backbone.EVENTS.STORYLINE_BLOCK_SELECTED, function (e) {
                self.m_selected_timeline_id = e.timeline;
                self.m_selected_channel_id = e.channel;
                self.m_selected_block_id = e.block;
                self._render();
            });
        },
        _editBlockProperties: function (slider, timelabel, default_value) {
            var self = this;
            var timeline = self.m_selected_timeline_id;
            var channel = self.m_selected_channel_id;
            var block = self.m_selected_block_id;
            var sq = self.m_sequencer;

            var newLength = self._updateTotalLength();
            var timeSpan = SecondsToTimeSpan(newLength);

            sq.editChannelBlock({
                timeline: timeline,
                channel: channel,
                block: block,
                timespan: timeSpan
            });

        },
        removeBlockProperties: function () {
            var self = this;
            $(self.el_id).empty();
        },
        _updateTotalLength: function () {
            var self = this;
            var hp = this.$el.find('input.h_placeholder');
            var mp = this.$el.find('input.m_placeholder');
            var sp = this.$el.find('input.s_placeholder');

            return StringFormattedTimeSpan2(hp.val(), mp.val(), sp.val());
        },
        _listenToEvents: function () {
            var self = this;
            amplify.subscribe(Backbone.EVENTS.ON_BLOCK_UPDATED, function (e) {
                var timelabel = self.$el.find('span.timelabel');
                var newLength = self._updateTotalLength();
                timelabel.empty();
                timelabel.append(newLength);

            });
        }


    });

    var BlockSelectorView = Backbone.View.extend(
    {
        initialize: function (options) {
            var self = this;
            this.options = options;
            self.m_selectcontrol = '#selectcontrol';
            self.m_template = '#selectcontrol_template';
            self.m_mediaselect = '#mediaselect';
            self.m_image_blocks_container = '#image_blocks_container';
            self.m_video_blocks_container = '#video_blocks_container';
            self.m_editor = self.options.editor;
            self.m_parent = $(self.el).parent();
        },
        events: { "click .return-selectcontrol": "returnToSelectControl" },
        returnToSelectControl: function (e) {
            var self = this;
            var selectControl = $(self.m_selectcontrol);
            $(selectControl).val(null);
            $(selectControl).find('.active')
                .removeClass('active');
        },
        _render: function () {
            var self = this;
            if (_.isUndefined(self.m_render)) {
                self.m_render = _.debounce(function () {
                    var selctrl = this.$el.find(self.m_selectcontrol);
                    selctrl.empty();
                    selctrl.append($(self.m_template).html());
                    selctrl.MultiColumnSelect({
                        menuclass: 'mcs',
                        openmenuClass: 'mcs-open',
                        openmenuText: 'Choose an Option ...',
                        containerClass: 'mcs-container',
                        itemClass: 'mcs-item', duration: 10,
                        onItemSelect: function () {
                            self._onMediaSelect(selctrl);
                        },
                    });
                }, 50);
            }
            self.m_render();
        },
        _onMediaSelect: function (selctrl) {
            var self = this;
            var val = this.$el.find(self.m_mediaselect).val();
            var elementID = this.$el.attr('id');
            switch (val) {
                case 'image': self._populateImageBlocks();
                    break;
                case 'video': self._populateVideoBlocks();
                    break;
            }
            _.each(selctrl.find('.mcs-item'), function (o, e) {
                var item = $(o).attr('data');
                $(o).attr('data-toggle', 'tab');
                $(o).attr('href', '#tab_' + item);
            });

        },
        _populateImageBlocks: function () {
            var self = this;
            var $container = $(self.m_image_blocks_container);
            $container.empty();
            $container.append($('#selectimage_template').html());
            $container.MultiColumnSelect({
                menuclass: 'mcs',
                openmenuClass: 'mcs-open',
                openmenuText: 'Choose an Option ...',
                containerClass: 'mcs-container',
                itemClass: 'mcs-item', duration: 10,
                onItemSelect: function () {
                    amplify.publish(Backbone.EVENTS.ON_BLOCK_ADDED, { aa: 'LALALA' });
                },
            });

        },
        _populateVideoBlocks: function () {
            var self = this;
            var $container = $(self.m_video_blocks_container);
            $container.empty();
            $container.append($('#selectvideo_template').html());
            $container.MultiColumnSelect({
                menuclass: 'mcs',
                openmenuClass: 'mcs-open',
                openmenuText: 'Choose an Option ...',
                containerClass: 'mcs-container',
                itemClass: 'mcs-item', duration: 10,
                onItemSelect: function () {
                    alert('Item is selected');
                },
            });
        },

    });

    $(function () {
        __JsonModel(<%= this.JsonData %>);

        var view = new CampaignsEditorView(
        {
            model: __jsonModel,
            el: '#campaign_editor'
        });

        view._render();

        var $widgets = $('.widget');

        $widgets.on("fullscreen.widgster", function () {
            $('.widget, .sidebar, .logo, .page-header, .page-title').not($(this)).fadeTo(150, 0);
        }).on("restore.widgster closed.widgster", function () {
            $('.widget, .sidebar, .logo, .page-header, .page-title').not($(this)).fadeTo(150, 1);
        });

        $widgets.widgster();


    });

</script>

<script id="selectcontrol_template" type="text/html">
    <select id="mediaselect" name="mediatype">
        <option value="image">Image</option>
        <option value="video">Video</option>
    </select>
</script>
<script id="selectvideo_template" type="text/html">
    <select id="videoselect" name="videotype">
        <option value="A">1</option>
        <option value="B">2</option>
        <option value="C">3</option>
        <option value="D">4</option>
        <option value="E">5</option>
        <option value="F">6</option>
        <option value="G">7</option>
    </select>
</script>
<script id="selectimage_template" type="text/html">
    <select id="imageselect" name="imagetype">
        <option value="1">A</option>
        <option value="2">B</option>
        <option value="3">C</option>
        <option value="4">D</option>
        <option value="5">E</option>
        <option value="6">F</option>
        <option value="7">G</option>
    </select>
</script>
<script id="returnbutton_template" type="text/html">
    <button type="button" id="returnbtn" class="btn btn-info btn-block">
        Return To Innocence!
    </button>
</script>

<script id="blocklist_container" type="text/template">
    <ul id="blockitems"></ul>
</script>

<script id="blocklist_template" type="text/html">
    <li><strong><@=content@></strong></li>
</script>

<script id="iblock" type="text/html">
    <div class="timelineBlock" style="<@=blockwidth@>" data-timeline_channel_block_id="<@= block_id @>">
        <button type="button" id="<@= block_id @>" class="btn btn-sm btn-info btn-block" style="height: 100%;"></button>
    </div>
</script>

<script id="channel_blocks_info_template" type="text/html">
    <h4 class="">Channel Blocks</h4>
    <ul id="channel_blocks_container" class="news-list">
        <@ _.each(blocksinfo,function (value,key) { @> 
            <li class="<@=key@>">
                <div class="icon pull-left">
                    <i class="<@= ResourceTypeToIcons(value.ResourceType)  @>"></i>
                </div>
                <div class="news-item-info">
                    <div class="name"><@= value.Name @></div>
                </div>
                <span class="timelabel" style="font-size: 20px; line-height: 18px;"><@= StringFormattedTimeSpan(value.Length)  @>
                </span>
            </li>
        <@ });@>
    </ul>
</script>

<script id="blockprops_template" type="text/html">
    <h4 class="">Block Properties
        &nbsp; &nbsp;
         
    </h4>
    <ul class="news-list">
        <li>

            <span class="timelabel"><@= timespan @>
            </span>

        </li>
        <li>
            <input type="text" class="h_placeholder news-item-info" />
        </li>
        <li>
            <input type="text" class="m_placeholder news-item-info" />
        </li>
        <li>
            <input type="text" class="s_placeholder news-item-info" />
        </li>
    </ul>
</script>

<script id="modal_confirm_template" type="text/html">
    <div class="modal fade" tabindex="-1" role="dialog" aria-labelledby="mModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="H1">Confirm
                    </h4>
                </div>
                <div class="modal-body">
                    <@= Question @>
                </div>
                <div class="modal-footer">
                    <button data-action="Yes" type="button" class="btn btn-primary" data-dismiss="modal">Yes</button>
                    <button data-action="No" type="button" class="btn btn-inverse" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
</script>

