 
Backbone.SERVICES = {};
Backbone.EVENTS = {};
Backbone.LOADING = {};
Backbone.CONSTS = {};

Backbone.EVENTS.ON_VIEWER_SELECTED = 'ON_VIEWER_SELECTED';
Backbone.CONSTS.VIEWER_SELECTABLE = 'VIEWER_SELECTABLE';
Backbone.CONSTS.ENTIRE_SELECTABLE = 'ENTIRE_SELECTABLE';

var ScreenTemplateFactory = Backbone.Controller.extend({

     
    initialize: function ()
    {

        
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

            case 'VIEWER_SELECTABLE' :
                {
                    this.m_useLabels = true;
                    this.m_mouseOverEffect = false;
                    this.m_selectableFrame = false;
                    this.m_selectablDivision = true;
                    break;
                }

            case 'ENTIRE_SELECTABLE' :
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

        /*Backbone.comBroker.fire(Backbone.EVENTS.ON_VIEWER_SELECTED, this, screenData);*/
        e.stopImmediatePropagation();
        return false;
    },

    /**
     Method is called when a single viewer (screen division) of the UI is clicked, in contrast to when the entire frame of the screen is selected.
     The difference in dispatch of the event depends on how the factory created this instance.
     @method _onScreenViewerSelected
     @param {Event} e
     @param {Object} i_caller
     @return {Boolean} false
     **/
    _onScreenViewerSelected: function (e, i_caller)
    {
         
        var element = e.target;

         
        if ($(element).data('for') != undefined) {
            var forDivison = $(element).data('for');
            element = $('#' + forDivison);
        }

        i_caller._deselectViewers();

        var campaign_timeline_board_viewer_id = $(element).data('campaign_timeline_board_viewer_id');
        var campaign_timeline_id = $(element).data('campaign_timeline_id');

        $(element).css({'fill': 'rgb(200,200,200)'});

        var screenData = {
            sd: $(element).data('sd'),
            campaign_timeline_board_viewer_id: campaign_timeline_board_viewer_id,
            campaign_timeline_id: campaign_timeline_id,
            elementID: i_caller.m_myElementID,
            owner: i_caller.getOwner(),
            screenTemplateData: self.m_screenTemplateData
        }

        /*Backbone.comBroker.fire(Backbone.EVENTS.ON_VIEWER_SELECTED, this, screenData);*/
        e.stopImmediatePropagation();
        return false;
    },

    /**
     Deselect all viewers, thus change their colors back to default.
     @method _deselectViewers
     @return none
     **/
    _deselectViewers: function () {
        var self = this;
        $(Elements.CLASS_SCREEN_DIVISION).each(function () {
            if ($(this).is('rect')) {
                $(this).css({'fill': 'rgb(230,230,230)'});
            }
        });
    },

    /**
     When enabled, _mouseOverEffect will highlight viewers when mouse is hovered over them.
     @method _mouseOverEffect
     @return none
     **/
    _mouseOverEffect: function () {
        var self = this;
        var a = $('#' + self.m_myElementID);
        var b = $('#' + self.m_myElementID).find('rect');
        $('#' + self.m_myElementID).find('rect').each(function () {
            $(this).on('mouseover', function () {
                $(this).css({'fill': 'rgb(190,190,190)'});
            }).mouseout(function () {
                $(this).css({'fill': 'rgb(230,230,230)'});
            });
        });
    },

    /**
     Get the owner (parent) of this instance, i.e., the one who created this.
     We use the owner attribute as a way to distinguish what type of instance this was created as.
     @method getOwner
     @return {Object} m_owner
     **/
    getOwner: function () {
        var self = this;
        return self.m_owner;
    },

    /**
     Create all the screen divisions (aka viewers) as svg snippets and push them into an array
     @method getDivisions
     @return {array} f array of all svg divisions
     **/
    getDivisions: function () {
        var self = this;
        var svg = self.create();
        return $(svg).find('rect');

        var f = $(svg).find('rect').map(function (k, v) {
            return '<svg style="padding: 0px; margin: 15px" width="20px" height="20px" xmlns="http://www.w3.org/2000/svg">  ' +
                '<g>' +
                v.outerHTML +
                '</g> ' +
                '</svg>';
        });
        return f;
    },

    /**
     Create will produce the actual SVG based Template (screen) with inner viewers and return HTML snippet to the caller.
     @method create
     @return {Object} html element produced by this factory
     **/
    create: function () {
        var self = this;
        var screensDivisons = '';
        var screenLabels = '';
        var i = 0;

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

    /**
     Begin listening to events and in turn set the behavior of the instance.
     @method activate
     @return none
     **/
    activate: function () {
        var self = this;

        if (self.m_mouseOverEffect)
            this._mouseOverEffect();

        if (self.m_selectableFrame)
            this.selectableFrame();

        if (self.m_selectablDivision)
            this.selectablelDivision();
    },

    /**
     When enabled, selectablelDivision will allow for UI mouse / click of individual viewers (screen divisions) and not entire frame.
     @method selectablelDivision
     @return none
     **/
    selectablelDivision: function () {
        var self = this;
        $(Elements.CLASS_SCREEN_DIVISION).on('click', function (e) {
            self._onScreenViewerSelected(e, self);
        });
    },

    /**
     When enabled, selectableFrame will allow for UI mouse / click of the outer frame of the template (screen) and not
     individual viewers.
     @method selectableFrame
     @return none
     **/
    selectableFrame: function () {
        var self = this;
         
        /*
        Backbone.comBroker.listen(Backbone.EVENTS.ON_VIEWER_SELECTED, function (e) {
            if (e.caller.elementID === self.m_myElementID) {
                $('#' + self.m_myElementID).find('rect').css({'stroke-width': '4', 'stroke': 'rgb(73,123,174)'});
            } else {
                $('#' + self.m_myElementID).find('rect').css({'stroke-width': '2', 'stroke': 'rgb(72,72,72)'});
            }
        });
        */

        $(Elements.CLASS_SCREEN_DIVISION).on('click', function (e)  {
             
            self._onScreenFrameSelected(e, self);
        });
    },

    /**
     The public method version of _deselectViewers, which de-selects all viewers
     @method deselectDivisons
     **/
    deselectDivisons: function () {
        var self = this;
        self._deselectViewers();
    },

    /**
     Select a division (aka viewer) using it's viewer_id, only applicable when class represents an actual timelime > board > viewer_id
     @method selectDivison
     @param {Number} i_campaign_timeline_board_viewer_id
     **/
    selectDivison: function (i_campaign_timeline_board_viewer_id) {
        var self = this;
        self._deselectViewers();
        var selectedElement = $('#' + self.m_myElementID).find('[data-campaign_timeline_board_viewer_id="' + i_campaign_timeline_board_viewer_id + '"]');
        $(selectedElement).css({'fill': 'rgb(200,200,200)'});
    },

    /**
     Release all members to allow for garbage collection.
     @method destroy
     @return none
     **/
    destroy: function () {
        var self = this;
        $(Elements.CLASS_SCREEN_DIVISION).off('click', function (e) {
            self._onScreenViewerSelected(e, self);
        });
        $(Elements.CLASS_SCREEN_DIVISION).off('click', function (e) {
            self._onScreenFrameSelected(e, self);
        });
        $(this).off('mouseover', function () {
            $(this).css({'fill': 'rgb(190,190,190)'});
        }).mouseout(function () {
            $(this).css({'fill': 'rgb(230,230,230)'});
        });
        $.each(self, function (k) {
            self[k] = undefined;
        });
    }
});

 