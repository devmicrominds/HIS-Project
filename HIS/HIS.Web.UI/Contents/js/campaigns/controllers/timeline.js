﻿Backbone.EVENTS.CAMPAIGN_TIMELINE_SELECTED = 'CAMPAIGN_TIMELINE_SELECTED';

var Timeline = Backbone.Controller.extend({

    /**
     Constructor
     @method initialize
     **/
    initialize: function () {
        var self = this;
        this.m_channels = {}; // hold references to all created channel instances
        this.m_screenTemplate = undefined;
        this.m_campaign_timeline_id = self.options.campaignTimelineID;
        this.m_timing = 'sequencer';
        this.m_stackViewID = undefined;
        this.m_property = Backbone.comBroker.getService(Backbone.SERVICES['PROPERTIES_VIEW']);
        this.m_sequences = Backbone.comBroker.getService(Backbone.SERVICES['SEQUENCER_VIEW']);
        this.m_selected = false;
        self._populateChannels();
        self._populateTimeline();
        self._listenInputChange();
        self._listenReset();
        this._onTimelineSelected();
        pepper.listenWithNamespace(Pepper.TEMPLATE_VIEWER_EDITED, self, $.proxy(self._templateViewerEdited, self));
        pepper.listenWithNamespace(Pepper.NEW_CHANNEL_ADDED, self, $.proxy(self._channelAdded, self));
    },

    /**
     Listen to reset of when switching to different campaign so we forget current state
     @method _listenReset
     **/
    _listenReset: function () {
        var self = this;
        Backbone.comBroker.listenWithNamespace(Backbone.EVENTS.CAMPAIGN_RESET, self, function(){
            self._reset();
        });
    },

    /**
     Reset current state
     @method _reset
     **/
    _reset: function(){
        var self = this;
        pepper.stopListenWithNamespace(Pepper.TEMPLATE_VIEWER_EDITED, self);
        pepper.stopListenWithNamespace(Pepper.NEW_CHANNEL_ADDED, self);
        Backbone.comBroker.stopListenWithNamespace(Backbone.EVENTS.CAMPAIGN_RESET, self);
        Backbone.comBroker.stopListenWithNamespace(Backbone.EVENTS.CAMPAIGN_TIMELINE_SELECTED, self);
        $(Elements.TIME_LINE_PROP_TITLE_ID).off("input", self.m_inputChangeHandler);
        $.each(self, function (k) {
            self[k] = undefined;
        });
    },

    /**
     Listen to timeline selection events and populate the properties panel accordingly.
     @method _onTimelineSelected
     @return none
     **/
    _onTimelineSelected: function () {
        var self = this;
        self.m_campaignTimelineSelectedHandler = function (e) {
            var timelineID = e.edata;
            if (self.m_campaign_timeline_id != timelineID) {
                self.m_selected = false;
                return;
            }
            self.m_selected = true;
            self._propLoadTimeline();
            // log('timeline selected ' + self.m_campaign_timeline_id);
        };
        Backbone.comBroker.listenWithNamespace(Backbone.EVENTS.CAMPAIGN_TIMELINE_SELECTED, self, self.m_campaignTimelineSelectedHandler);
    },

    /**
     Update msdb when the timeline title has changed.
     @method _listenInputChange
     @return none
     **/
    _listenInputChange: function () {
        var self = this;
        self.m_inputChangeHandler = _.debounce(function (e) {
            if (!self.m_selected)
                return;
            pepper.setCampaignTimelineRecord(self.m_campaign_timeline_id, 'timeline_name', $(Elements.TIME_LINE_PROP_TITLE_ID).val());
        }, 150, false);
        $(Elements.TIME_LINE_PROP_TITLE_ID).on("input", self.m_inputChangeHandler);
    },

    /**
     When a campaign_timeline_board_template is edited, modify its related UI (inside sequencer)
     @method campaign_timeline_board_template_id
     @param {event} e template viewer ids
     **/
    _templateViewerEdited: function (e) {
        var self = this;
        if (!self.m_selected)
            return;
        self._populateBoardTemplate(e.edata.campaign_timeline_board_template_id);
    },

    /**
     New channel was added to an existing timeline (most likely through the addition of a viewer (screen division) template editor)
     @method _channelAdded
     @param {event} e
     **/
    _channelAdded: function (e) {
        var self = this;
        if (!self.m_selected)
            return;
        self.m_channels[e.edata.chanel] = new Channel({campaignTimelineChanelID: e.edata.chanel});
    },

    /**
     Populate the timeline property
     @method _listenInputChange
     **/
    _propLoadTimeline: function () {
        var self = this;
        self.m_property.viewPanel(Elements.TIMELINE_PROPERTIES);
        var recTimeline = pepper.getCampaignTimelineRecord(self.m_campaign_timeline_id);
        $(Elements.TIME_LINE_PROP_TITLE_ID).val(recTimeline['timeline_name']);
        self._populateTimelineLength();
    },

    /**
     Populate the timeline length in its properties box
     @method _populateTimelineLength
     **/
    _populateTimelineLength: function () {
        var self = this;
        self.m_xdate = Backbone.comBroker.getService('XDATE');
        var totalDuration = parseInt(pepper.getTimelineTotalDuration(self.m_campaign_timeline_id));
        totalDuration = self.m_xdate.clearTime().addSeconds(totalDuration).toString('HH:mm:ss');
        $(Elements.TIMELINE_LENGTH).text(totalDuration);
    },

    /**
     Create the timeline and load up its template (screen divisions) UI
     @method _populateTimeline
     @return none
     **/
    _populateTimeline: function () {
        var self = this;
        var boardTemplateIDs = pepper.getTemplatesOfTimeline(self.m_campaign_timeline_id);
        for (var i = 0; i < boardTemplateIDs.length; i++) {
            self._populateBoardTemplate(boardTemplateIDs[i]);
        }
    },

    /**
     Create a channel instance for every channel this timeline hosts
     @method _populateChannels
     @return none
     **/
    _populateChannels: function () {
        var self = this;
        var channelIDs = pepper.getChannelsOfTimeline(self.m_campaign_timeline_id);
        for (var i = 0; i < channelIDs.length; i++) {
            self.m_channels[channelIDs[i]] = new Channel({campaignTimelineChanelID: channelIDs[i]});
        }
    },

    /**
     Load up the board template (screen divisions) for this timeline instance.
     In case sequencer is used, we push it to the sequencer, thus creating the thumbnail template
     inside the sequencer so this timeline can be selected.
     Scheduler future support.
     @method _populateBoardTemplate
     @param {Number} i_campaign_timeline_board_template_id
     @return none
     **/
    _populateBoardTemplate: function (i_campaign_timeline_board_template_id) {
        var self = this;

        var recBoard = pepper.getGlobalBoardRecFromTemplate(i_campaign_timeline_board_template_id);
        var width = parseInt(recBoard['board_pixel_width']);
        var height = parseInt(recBoard['board_pixel_height']);

        Backbone.comBroker.getService(Backbone.SERVICES.RESOLUTION_SELECTOR_VIEW).setResolution(width + 'x' + height);
        if (width > height) {
            Backbone.comBroker.getService(Backbone.SERVICES.ORIENTATION_SELECTOR_VIEW).setOrientation(Backbone.CONSTS.HORIZONTAL);
        } else {
            Backbone.comBroker.getService(Backbone.SERVICES.ORIENTATION_SELECTOR_VIEW).setOrientation(Backbone.CONSTS.VERTICAL);
        }
        var screenProps = pepper.getTemplateViewersScreenProps(self.m_campaign_timeline_id, i_campaign_timeline_board_template_id)

        // Future support for scheduler
        switch (self.m_timing) {
            case 'sequencer':
                {
                    self.m_sequences.createTimelineThumbnailUI(screenProps);
                    break;
                }

            case 'scheduler':
                {
                    break;
                }
        }
    },

    /**
     Create the actual UI for this timeline instance. We use the ScreenTemplateFactory for SVG creation
     and insert the snippet onto timelineViewStack so the timeline UI can be presented when selected.
     @method _createTimelineUI
     @param {Object} i_screenProps template properties object
     @return none
    _createTimelineUI: function (i_screenProps) {
        var self = this;

        var screenTemplateData = {
            orientation: Backbone.comBroker.getService(Backbone.SERVICES.ORIENTATION_SELECTOR_VIEW).getOrientation(),
            resolution: Backbone.comBroker.getService(Backbone.SERVICES.RESOLUTION_SELECTOR_VIEW).getResolution(),
            screenProps: i_screenProps,
            scale: '7'
        };

        var screenTemplate = new ScreenTemplateFactory({
            i_screenTemplateData: screenTemplateData,
            i_type: ScreenTemplateFactory.VIEWER_SELECTABLE,
            i_owner: this});

        var snippet = screenTemplate.create();
        // var elemID = $(snippet).attr('id');
        var divID1 = 'selectableScreenCollections' + _.uniqueId();
        var divID2 = 'selectableScreenCollections' + _.uniqueId();

        var snippetWrapper = '<div id="' + divID1 + '" style="display: none">' +
            '<div align="center" >' +
            '<div id="' + divID2 + '" align="center">' +
            '</div>' +
            '</div>' +
            '</div>';

        $(Elements.SELECTED_TIMELINE).append(snippetWrapper);

        var timelineViewStack = Backbone.comBroker.getService(Backbone.SERVICES.CAMPAIGN_VIEW).getTimelineViewStack();
        $('#' + divID2).append($(snippet));
        screenTemplate.selectablelDivision();
        var view = new Backbone.View({el: '#' + divID1});

        // if we are updating layout from ScreenLayoutEditorView (but actually creating a new Template layout)
        // we remove the previous Template Layout from DOM as well as its matching ScreenTemplateFactory instance
        if (self.m_stackViewID) {
            $('#' + self.m_stackViewID).remove();
            self.m_screenTemplate.destroy();
        }
        ;

        self.m_screenTemplate = screenTemplate;
        self.m_stackViewID = timelineViewStack.addView(view);
        screenTemplate.activate();
    },
     **/

    /**
     Return the view stack index this timeline occupies in the timelineViewStack manager.
     @method getStackViewID
     @return {Number} m_stackViewID
    getStackViewID: function () {
        var self = this;
        return self.m_stackViewID;
    },
     **/

    /**
     The timeline hold references to all of the channels it creates that exist within it.
     The getChannelInstance returns a specific channel instance for a channel_id.
     @method getChannelInstance
     @param {Number} i_campaign_timeline_chanel_id
     @return {Object} Channel
     **/
    getChannelInstance: function (i_campaign_timeline_chanel_id) {
        var self = this;
        return self.m_channels[i_campaign_timeline_chanel_id];
    },

    /**
     Delete this timeline thus also need to delete all of its related channels
     @method deleteTimeline
     @return none
     **/
    deleteTimeline: function () {
        var self = this;
        var boardTemplateID = pepper.getGlobalTemplateIdOfTimeline(self.m_campaign_timeline_id);
        pepper.removeTimelineFromCampaign(self.m_campaign_timeline_id);
        var campaignTimelineBoardTemplateID = pepper.removeBoardTemplateFromTimeline(self.m_campaign_timeline_id);
        pepper.removeBoardTemplate(boardTemplateID);
        pepper.removeTimelineBoardViewerChannels(campaignTimelineBoardTemplateID);
        pepper.removeBoardTemplateViewers(boardTemplateID);
        for (var channel in self.m_channels) {
            self.m_channels[channel].deleteChannel();
            delete self.m_channels[channel];
        }
        self._reset();
    }
});

 