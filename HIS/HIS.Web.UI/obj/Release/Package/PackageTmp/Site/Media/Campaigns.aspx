<%@ Page Title="" Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="false" CodeBehind="Campaigns.aspx.cs" Inherits="HIS.Web.UI.Site.Media.Campaigns" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-md-12">
            <h2 class="page-title">Campaigns 
            <small></small>
            </h2>
        </div>
    </div>
     
    <sym:CallbackPanel ID="ListPanel" runat="server" ClientCallBackFunction="campaigns.Callback" containertriggers-capacity="4">
        <asp:PlaceHolder ID="ListPlaceHolder" runat="server"></asp:PlaceHolder>
    </sym:CallbackPanel>

    <script type="text/javascript" src="/Contents/lib/jquery.slimscroll.min.js"> </script>
    <script type="text/javascript" src="/Contents/lib/jquery.mcustomscrollbar.min.js"> </script>
     
        

    <script type="text/javascript">

        __JsonModel(<%= JsonData %>); 
        var ScreenTemplate = __jsonModel;

        var ScreenTemplateFactory = Backbone.Controller.extend({
            initialize: function ()
            {

                var self = this;

                this.m_owner = self.options.i_owner;
                
                this.m_screenTemplateData = self.options.i_screenTemplateData;
                this.m_orientation = self.options.i_screenTemplateData['orientation'];
                this.m_resolution = self.options.i_screenTemplateData['resolution'];
                this.m_screenProps = self.options.i_screenTemplateData['screenProps'];
                this.m_scale = self.options.i_screenTemplateData['scale'];
                this.m_svgWidth = (this.m_resolution.split('x')[0]) / this.m_scale;
                this.m_svgHeight = (this.m_resolution.split('x')[1]) / this.m_scale;

                this.m_myElementID = 'svgScreenLayout' + '_' + _.uniqueId();

                switch (self.options.i_type) {

                    case 'VIEWER_SELECTABLE':
                        {
                            this.m_useLabels = true;
                            this.m_mouseOverEffect = false;
                            this.m_selectableFrame = false;
                            this.m_selectablDivision = true;
                            break;
                        }

                    case 'ENTIRE_SELECTABLE':
                        {

                            this.m_useLabels = false;
                            this.m_mouseOverEffect = true;
                            this.m_selectableFrame = true;
                            this.m_selectablDivision = false;
                            break;
                        }
                }
            },
            _onScreenFrameSelected: function (e, i_caller) {
                var self = i_caller;
                var element = e.target;
                var campaign_timeline_board_viewer_id = $(element).data('campaign_timeline_board_viewer_id');
                var campaign_timeline_id = $(element).data('campaign_timeline_id');

                var screenData = {
                    sd: $(element).data('sd'),
                    elementID: i_caller.m_myElementID,
                    owner: i_caller.getOwner(),
                    campaign_timeline_board_viewer_id: campaign_timeline_board_viewer_id,
                    campaign_timeline_id: campaign_timeline_id,
                    screenTemplateData: self.m_screenTemplateData
                };

                self._deselectViewers();
                amplify.publish(Backbone.EVENTS.ON_VIEWER_SELECTED, screenData);
                e.stopImmediatePropagation();
                return false;
            },
            _onTimelineSelected: function (e, i_caller) {
                 
                var self = i_caller;
                var element = e.target;
                var campaign_timeline_board_viewer_id = $(element).data('campaign_timeline_board_viewer_id');
                var campaign_timeline_id = $(element).data('campaign_timeline_id');

                var screenData = {
                    sd: $(element).data('sd'),
                    elementID: i_caller.m_myElementID,
                    owner: i_caller.getOwner(),
                    campaign_timeline_board_viewer_id: campaign_timeline_board_viewer_id,
                    campaign_timeline_id: campaign_timeline_id,
                    screenTemplateData: self.m_screenTemplateData
                };

                self._deselectViewers();
                amplify.publish(Backbone.EVENTS.ON_TIMELINE_SELECTED, {
                    timelineID: campaign_timeline_id,
                    screenTemplate: self
                });
                 
                e.stopImmediatePropagation();
                return false;
            },
            _onScreenViewerSelected: function (e, i_caller) {

                var element = e.target;
                if ($(element).data('for') != undefined) {
                    var forDivison = $(element).data('for');
                    element = $('#' + forDivison);
                }

                i_caller._deselectViewers();

                var campaign_timeline_board_viewer_id = $(element).data('campaign_timeline_board_viewer_id');
                var campaign_timeline_id = $(element).data('campaign_timeline_id');

                $(element).css({ 'fill': 'rgb(200,200,200)' });

                var screenData = {
                    sd: $(element).data('sd'),
                    campaign_timeline_board_viewer_id: campaign_timeline_board_viewer_id,
                    campaign_timeline_id: campaign_timeline_id,
                    elementID: i_caller.m_myElementID,
                    owner: i_caller.getOwner(),
                    screenTemplateData: self.m_screenTemplateData
                }
                amplify.publish(Backbone.EVENTS.ON_VIEWER_SELECTED, screenData);
                e.stopImmediatePropagation();
                return false;
            },
            _deselectViewers: function () {
                var self = this;
                $(Elements.CLASS_SCREEN_DIVISION).each(function () {
                    if ($(this).is('rect')) {
                        $(this).css({ 'fill': 'rgb(230,230,230)' });
                    }
                });
            },
            _mouseOverEffect: function () {
                var self = this;
                var a = $('#' + self.m_myElementID);
                var b = $('#' + self.m_myElementID).find('rect');
                $('#' + self.m_myElementID).find('rect').each(function () {
                    $(this).on('mouseover', function () {
                        $(this).css({ 'fill': 'rgb(190,190,190)' });
                    }).mouseout(function () {
                        $(this).css({ 'fill': 'rgb(230,230,230)' });
                    });
                });
            },
            getOwner: function () {
                var self = this;
                return self.m_owner;
            },
            getDivisions: function () {
                var self = this;
                var svg = self.create();
                return $(svg).find('rect');

                var f = $(svg).find('rect').map(function (k, v) {
                    return '<svg class="timeline_svg" style="padding: 0px; margin: 15px" width="20px" height="20px" xmlns="http://www.w3.org/2000/svg">  ' +
                        '<g>' +
                        v.outerHTML +
                        '</g> ' +
                        '</svg>';
                });
                return f;
            },
            create: function () {
                var self = this;
                var screensDivisons = '';
                var screenLabels = '';
                var i = 0;
                var data_campaign_timeline_id = '';

                for (var screenValues in self.m_screenProps) {
                    i++;
                    
                    var screenValue = self.m_screenProps[screenValues];
                     
                    var x = screenValue['x'] == 0 ? 0 : screenValue['x'] / self.m_scale;
                    var y = screenValue['y'] == 0 ? 0 : screenValue['y'] / self.m_scale;
                    var w = screenValue['w'] == 0 ? 0 : screenValue['w'] / self.m_scale;
                    var h = screenValue['h'] == 0 ? 0 : screenValue['h'] / self.m_scale;
                    var campaign_timeline_board_viewer_id = screenValue['campaign_timeline_board_viewer_id'];
                    var campaign_timeline_id = screenValue['campaign_timeline_id'];
                    
                    var sd = screenValues;

                    var uniqueID = 'rectSD' + '_' + _.uniqueId();

                    if (self.m_useLabels == true)
                        screenLabels += '<text class="screenDivisionClass"' + '" data-for="' + uniqueID + '" x="' + (x + (w / 2)) + '" y="' + (y + (h / 2)) + '" font-family="sans-serif" font-size="12px" text-anchor="middle" alignment-baseline="middle" fill="#666">' + i + '</text>';


                    screensDivisons += '<rect id="' + uniqueID +
                        '" data-campaign_timeline_board_viewer_id="' + campaign_timeline_board_viewer_id +
                        '" data-campaign_timeline_id="' + campaign_timeline_id +
                        '" x="' + x +
                        '" y="' + y +
                        '" width="' + w +
                        '" height="' + h +
                        '" data-sd="' + sd +
                        '" class="screenDivisionClass"' +
                        '  style="fill:rgb(230,230,230);stroke-width:2;stroke:rgb(170,170,170)"/>';

                    data_campaign_timeline_id = campaign_timeline_id;
                }

                return ($('<svg class="timeline_svg" style="padding: 0px;cursor:pointer; margin: 15px" id="' + self.m_myElementID + '" data-timeline-id="' + data_campaign_timeline_id + '" width="' + self.m_svgWidth + '" height="' + self.m_svgHeight + '" xmlns="http://www.w3.org/2000/svg">  ' +
                    '<g>' +
                    screensDivisons +
                    screenLabels +
                    '</g> ' +
                    '</svg>'));
            },
            activate: function () {
                var self = this;

                if (self.m_mouseOverEffect)
                    this._mouseOverEffect();

                console.log(self.m_selectableFrame);
                if (self.m_selectableFrame)
                    this.selectableFrame();

                if (self.m_selectablDivision)
                    this.selectablelDivision();
            },
            selectablelDivision: function () {
                var self = this;
                $(Elements.CLASS_SCREEN_DIVISION).on('click', function (e) {
                    self._onScreenViewerSelected(e, self);
                });
            },
            selectableFrame: function ()
            {
                var self = this;
                amplify.subscribe(Backbone.EVENTS.ON_VIEWER_SELECTED, function (e)  {
                    if (e.elementID === self.m_myElementID) {
                        $('#' + self.m_myElementID).find('rect').css({ 'stroke-width': '4', 'stroke': 'rgb(73,123,174)' });                        

                    } else {

                        $('#' + self.m_myElementID).find('rect').css({ 'stroke-width': '2', 'stroke': 'rgb(170,170,170)' });
                    }
                     
                }); 

                $(Elements.CLASS_SCREEN_DIVISION).on('click', function (e) {
                    self._onScreenFrameSelected(e, self);
                    amplify.publish(Backbone.EVENTS.ON_SCREEN_TEMPLATE_SELECTED, self.m_screenTemplateData);
                });

            },
            selectTimeline: function (e, i_caller) {
                var self = this;
                self._onTimelineSelected(e, i_caller);
            },
            frameSelectable: function () {
                var self = this;
                amplify.subscribe(Backbone.EVENTS.ON_VIEWER_SELECTED, function (e) {
                    if (e.elementID === self.m_myElementID) {
                        $('#' + self.m_myElementID).find('rect').css({ 'stroke-width': '4', 'stroke': 'rgb(73,123,174)' });
                    } else {
                        $('#' + self.m_myElementID).find('rect').css({ 'stroke-width': '2', 'stroke': 'rgb(170,170,170)' });
                    }

                });
                $(Elements.CLASS_SCREEN_DIVISION).on('click', function (e) {
                    self._onScreenFrameSelected(e, self);
                    self._onTimelineSelected(e, self);
                });
            },
            deselectDivisons: function () {
                var self = this;
                self._deselectViewers();
            },
            selectDivison: function (i_campaign_timeline_board_viewer_id)
            {
                var self = this;
                self._deselectViewers();
                var selectedElement = $('#' + self.m_myElementID)
                    .find('[data-campaign_timeline_board_viewer_id="' + 
                    i_campaign_timeline_board_viewer_id + '"]');

                
                $(selectedElement).css({ 'fill': '#999' });
            },
            destroy: function () {
                var self = this;
                $(Elements.CLASS_SCREEN_DIVISION).off('click', function (e) {
                    self._onScreenViewerSelected(e, self);
                });
                $(Elements.CLASS_SCREEN_DIVISION).off('click', function (e) {
                    self._onScreenFrameSelected(e, self);
                });
                $(this).off('mouseover', function () {
                    $(this).css({ 'fill': 'rgb(190,190,190)' });
                }).mouseout(function () {
                    $(this).css({ 'fill': 'rgb(230,230,230)' });
                });
                $.each(self, function (k) {
                    self[k] = undefined;
                });
            }
        });

        var campaigns =
        {
            Callback: function (e) {
                    
            }
        };

    </script>

    <script id="save_campaign_button_template" type="text/html">
        <button id="saveCampaignBtn" type="button" class="btn btn-success col-md-2">
            Save Campaign
        </button>
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
            <div class="storylineChannel" >
                <div class="channelHead" style="text-align:center">
                    <@= value @>       
                </div>
                <div class="channelBodyContainer" style="width:95%">
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
</asp:Content>
