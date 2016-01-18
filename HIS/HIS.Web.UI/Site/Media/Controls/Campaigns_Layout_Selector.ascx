<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Campaigns_Layout_Selector.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Campaigns_Layout_Selector" %>
<div class="row">
    <div class="col-md-9">
        <section class="widget large">
            <header>
                <fieldset>
                    <legend>Layouts</legend>
                </fieldset>
            </header>
            <div class="body">
                <div id="layout_selector">
                    <div id="screen_templates" class="well well-large">
                    </div>
                </div>
            </div>
        </section>
    </div>
</div>
<script type="text/javascript">

    __JsonModel(<%= this.JsonData %>);

    var ScreenLayoutSelectorView = Backbone.View.extend(
    {
        initialize: function () {
            var self = this;

        },
        render: function () {
            var self = this;
            var screentemplates = self.$el.find('#screen_templates');
            screentemplates.empty();
            var template = self.model.Data.ScreenTemplate;
            var orientation = template.Orientation;
            var resolution = template.Resolution;

            for (var screenType in ScreenTemplate[orientation][resolution]) {
                var screenTemplateData = {
                    orientation: orientation,
                    resolution: resolution,
                    screenType: screenType,
                    screenProps: ScreenTemplate[orientation][resolution][screenType],
                    scale: 14
                };

                var screenTemplate = new ScreenTemplateFactory({
                    i_screenTemplateData: screenTemplateData,
                    i_type: Backbone.CONSTS.ENTIRE_SELECTABLE,
                    i_owner: self
                });

                var snippet = screenTemplate.create();
                screentemplates.append($(snippet));
                screenTemplate.activate();

            }

            self._subscribe();
        },
        _subscribe: function () {
            var self = this;
            amplify.subscribe(Backbone.EVENTS.ON_SCREEN_TEMPLATE_SELECTED, function screenTemplateSelected(obj)
            {
               var oData =
               {
                   Campaign: self.model.Data.Campaign,
                   ScreenType: obj.screenType, 
               };

                amplify.request(Backbone.REQUEST.ADD_CAMPAIGNS_TIMELINE, JSON.stringify(oData), function (data) {

                   
                    var oJson = {};
                    oJson.Action = 'EditCampaign';
                    oJson.DateTimeRequest = moment().format();
                    oJson.DataTarget = self.model.Data.Campaign;
                    oJson.Timeline = data.Timeline;
                    _Post(document.URL, oJson);

                });


                amplify.unsubscribe(Backbone.EVENTS.ON_SCREEN_TEMPLATE_SELECTED, screenTemplateSelected);
            });
        },
        

    });

    $(function () {

        var screenLayoutSelector = new ScreenLayoutSelectorView(
        {
            el: '#layout_selector',
            model: __jsonModel
        });

        screenLayoutSelector.render();
    });

</script>
