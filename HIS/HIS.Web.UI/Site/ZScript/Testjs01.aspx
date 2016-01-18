<%@ Page Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="true" CodeBehind="Testjs01.aspx.cs" Inherits="HIS.Web.UI.Site.ZScript.Testjs01" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="page-title">My Js Test
            <small></small>
            </h2>
        </div>
    </div>
    <sym:callbackpanel id="ControlPanel" runat="server" clientcallbackfunction="ucCallback" containertriggers-capacity="4">
        <asp:PlaceHolder ID="ControlPlaceHolder" runat="server"></asp:PlaceHolder>
    </sym:callbackpanel>

    <script type="text/javascript">

        __JsonModel(<%= JsonData %>);
        var ScreenTemplate = __jsonModel;

        var ucCallback = function () {
            console.log('callback!');
        };

        var MyScreenTemplateFactory = Backbone.Controller.extend({
            initialize: function () {
                var self = this;
                this.m_owner = self.options.i_owner;
                this.m_myElementID = 'svgScreenLayout' + '_' + _.uniqueId();
                this.m_screenTemplateData = self.options.i_screenTemplateData;
                this.m_orientation = self.options.i_screenTemplateData['orientation'];
                this.m_resolution = self.options.i_screenTemplateData['resolution'];
                this.m_screenProps = self.options.i_screenTemplateData['screenProps'];
                this.m_scale = self.options.i_screenTemplateData['scale'];
                this.m_svgWidth = (this.m_resolution.split('x')[0]) / this.m_scale;
                this.m_svgHeight = (this.m_resolution.split('x')[1]) / this.m_scale;

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
            create: function () {
                var self = this;
                var screensDivisons = '';
                var screenLabels = '';
                var i = 0;
                console.log("***************************MyScreenTemplateFactory:create:  self.m_screenProps************************************************");
                console.log(self.m_screenProps);
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
                        '  style="fill:rgb(230,230,230);stroke-width:2;stroke:rgb(72,72,72)"/>';
                }

                return ($('<svg style="padding: 0px; margin: 15px" id="' + self.m_myElementID + '" width="' + self.m_svgWidth + '" height="' + self.m_svgHeight + '" xmlns="http://www.w3.org/2000/svg">  ' +
                    '<g>' +
                    screensDivisons +
                    screenLabels +
                    '</g> ' +
                    '</svg>'));
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
        });



    </script>
</asp:Content>
