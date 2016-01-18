<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Campaigns_List.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Campaigns_List" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">
            <div id="campaigns_menu" class="mailbox-content tabbable tab-right">
                <ul class="nav nav-pills">
                    <li class="active"><a href="#tab_view" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i></a></li>
                    <li><a href="#tab_add" data-toggle="tab"><i class="glyphicon glyphicon-plus"></i></a></li>
                </ul>
            </div>
            <div class="tab-content">
                <div id="tab_view" class="tab-pane fade active in">
                    <asp:GridView ID="__grid" ItemType="HIS.Data.Campaigns" CssClass="gridview table-striped table-hover" AutoGenerateColumns="false"
                        ShowHeader="false" AlternatingRowStyle-BackColor="White" AllowPaging="true" AllowCustomPaging="true"
                        ShowFooter="true"
                        runat="server">
                        <PagerSettings Position="Top" Visible="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="#" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <%# GridViewHelper.GetGridViewRowIndex(this.__grid,Container.DataItemIndex) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" ItemStyle-Width="80%">
                                <ItemTemplate>
                                    <div>
                                        <ul class="activity-list">
                                            <li>
                                                <p class="lead"><%# Item.Name %></p>
                                            </li>
                                            <li><em><%# Item.Description %></em></li>
                                            <li>
                                                <label><%# Item.ScreenTemplate.Resolution.Resolution %></label></li>
                                            <li>
                                                <label><%# Item.ScreenTemplate.Resolution.Orientation.Orientation.ToString() %></label></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <button data-uid="<%# Item.Id %>" type="button" class="editcampaign btn btn-small btn-info">
                                        <i class="fa fa-edit"></i>
                                    </button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <sym:Pager ID="Pager1" runat="server" PagerContext="media.campaigns" />
                        </PagerTemplate>
                        <EmptyDataTemplate>
                            There are currently no items in this table.
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                <div id="tab_add" class="tab-pane fade in">
                    <div id="wizard" class="form-wizard">
                        <ul class="nav nav-tabs" style="display: none;">
                            <li class="active"><a data-toggle="tab" href="#tabstart">Start</a></li>
                            <li><a data-toggle="tab" href="#taborient">Orientation</a></li>
                            <li><a data-toggle="tab" href="#tabtpl">Templates</a></li>
                        </ul>
                        <div class="tab-content">
                            <div id="tabstart" class="tab-pane fade in active">
                                <div class="well well-hazy">
                                    <div id="campaign_form" class="form-horizontal">
                                        <fieldset>
                                            <legend style="padding-left: 10px;">New Campaign </legend>
                                            <div class="control-group">
                                                <label class="control-label" for="campaign_name">Name</label>
                                                <div class="controls form-group">
                                                    <div class="col-md-6">
                                                        <input class="m_name form-control" type="text" placeholder="Campaign Name"
                                                             maxlength="25" data-validation="required"
                                                             autocomplete="off" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label" for="description">Description</label>
                                                <div class="controls form-group">
                                                    <div class="col-md-8">
                                                        <input id="description" class="m_description form-control" type="text" placeholder="Campaign Description"
                                                             maxlength="25" data-validation="required" 
                                                            autocomplete="off"/>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <br />
                                    </div>
                                    <div class="form-actions">                                         
                                        <button type="button" data-action="start_next" data-toggle="tab" title="Next" class="btn btn-info"  data-target="#taborient">
                                             Next &nbsp; 
                                        </button>
                                    </div>
                                </div> 
                            </div>
                            <div id="taborient" class="tab-pane fade in">
                                <div id="orientationView">
                                    <div class="well well-hazy">
                                        <fieldset>
                                            <legend style="padding-left: 10px;">Screen Orientation</legend>
                                            <div class="controls-group">
                                                <div class="row thumbnails">
                                                    <div class="col-md-6">
                                                        <a data-toggle="tab" data-target="#tabresxH" style="cursor:pointer;">
                                                            <div class="well well-sm well-white  text-center">
                                                                <img id="imageH" src="/Contents/img/orientationH.png" style="width: 35%; height: 35%;" />
                                                            </div>
                                                        </a>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <a data-toggle="tab" data-target="#tabresxV" style="cursor:pointer;" >
                                                            <div class="well well-sm well-white text-center">
                                                                <img id="imageV" src="/Contents/img/orientationV.png" style="width: 35%; height: 35%;" />
                                                            </div>
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                            <button data-target="#tabstart" data-toggle="tab" title="Previous" type="button" class="btn btn-info btn-block">
                                                <i class="fa fa-chevron-left"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tabresxH" class="tab-pane fade in">
                                <div class="well well-hazy">
                                    <div id="resolutionViewH" class="form-horizontal">
                                        <fieldset>
                                            <legend style="padding-left: 10px;">Screen Resolution</legend>
                                            <div class="control-group">
                                                <label class="control-label" for="resolution">Resolution</label>
                                                <div id="Hplaceholder" class="controls form-group"></div>
                                            </div>
                                        </fieldset>
                                        <br />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <button data-target="#taborient" data-toggle="tab" title="Previous" type="button" class="btn btn-info btn-block">
                                            <i class="fa fa-chevron-left"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div id="tabresxV" class="tab-pane fade in" style="">
                                <div id="resolutionViewV" class="form-horizontal">
                                    <fieldset>
                                        <legend style="padding-left: 10px;">Screen Resolution</legend>
                                        <div class="control-group">
                                            <label class="control-label" for="resolution">Resolution</label>
                                            <div id="Vplaceholder" class="controls form-group"></div>
                                        </div>
                                    </fieldset>
                                    <br />
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <button data-target="#taborient" data-toggle="tab" title="Previous" type="button" class="btn btn-info btn-block">
                                            <i class="fa fa-chevron-left"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div id="tabtpl" class="tab-pane fade in">
                                <div class="form-horizontal">
                                    <fieldset>
                                        <legend>Layouts</legend>
                                        <div id="screen_templates"></div>
                                    </fieldset> 
                                </div>
                            </div>
                            <div id="tablast" class="tab-pane">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</div>

<script type="text/javascript">

    var CampaignModel = Backbone.Epoxy.Model.extend({
        defaults:
        {
            Name: undefined,
            Description: undefined,
            Orientation: undefined,
            Resolution: undefined,
            ScreenTemplate: undefined,
        },
    });


    var CampaignsMenuView = Backbone.View.extend(
   {
       initialize: function ()
       {
           var self = this;
           /*
            self.verticalbase = $("#Vplaceholder");
            self.horizontalbase = $("#Hplaceholder");
            */
            self.campaignWizard = new CampaignWizardView({
                el: $('#wizard'),
                model: new CampaignModel(),
            });
        },
        events: {
            "show.bs.tab a[href=#tab_add]": "wiredUp",
        },
       
        setupVBase: function () {
            /*
            var self = this;
            self.verticalbase.empty();
            self.verticalbase.append(_.template($('#selresV_template').html(), {
                selresV_id: 'selresV'
            }));
            $('#selresV').selectpicker();
            */
        },
        setupHBase: function () {
            /*
            var self = this;
            self.horizontalbase.empty();
            self.horizontalbase.append(_.template($('#selresH_template').html(), {
                selresH_id: 'selresH'
            }));
            $('#selresH').selectpicker();
            */
        },
        wiredUp: function (event) {
            var self = this;
            self.campaignWizard.resetTab();
            self.setupHBase();
            self.setupVBase(); 
        },
        render: function () {
            var self = this;
            self.campaignWizard.render();
        },
        destroy: function () {
            var self = this;
            self.campaignWizard.destroy(); 

        }

    });

   
    var CampaignWizardView = Backbone.Epoxy.View.extend(
    {
        initialize: function ()   {
            var self = this;
            self.current_tab = "#tabstart"; 
        },
        events:
        {
            "shown.bs.tab button[data-toggle=tab]": "currentTabChanged",
            "shown.bs.tab a[data-toggle=tab]":"currentTabChanged",  
            "change #selresV": "selectResolutionChanged",
            "change #selresH": "selectResolutionChanged",
            'click button[data-action=start_next]': "startNextClick",
        }, 
        bindings: {
            'input.m_name': 'value:Name,events:["keyup"]',
            'input.m_description': 'value:Description,events:["keyup"]', 
        },
        startNextClick: function (e) {
            var self = this;
            if (__isNue(self.model.get("Name")) || __isNue(self.model.get("Description"))) {
                alert('Please fill in the form!');
                e.stopImmediatePropagation();
            }

        },
        selectResolutionChanged: function (event) {
            var self = this;
            var e = event.target; 
            $('#wizard .tab-pane').removeClass('active').removeClass('in');
            $('#tabtpl').addClass('active').addClass('in'); 
            var resolution = $(e).val();
            self.model.set("Resolution", resolution);
            new ScreenTemplateSelectorView({ orientation:self.model.get("Orientation"), resolution: self.model.get("Resolution"),
                wizard:self,
            }).render();
             
        },
        currentTabChanged: function (e) {
            var self = this;
            var el = e.target;
            self.current_tab = $(el).data('target');
            switch (self.current_tab)
            {
                case "#tabresxH":
                    self.model.set("Orientation", 'HORIZONTAL');
                    amplify.request(Backbone.REQUEST.GET_SCREEN_RESOLUTIONS, JSON.stringify({ Orientation: 1 }), function (data, status) {
                        var elo = self.$el.find('#Hplaceholder');
                        elo.empty();
                        elo.append(_.template($('#selresH_template').html(), {
                            selresH_id: 'selresH',resolutions:data
                        }));
                        $('#selresH').selectpicker();
                        
                    });
                    break;
                case "#tabresxV":
                    self.model.set("Orientation", 'VERTICAL');
                    amplify.request(Backbone.REQUEST.GET_SCREEN_RESOLUTIONS, JSON.stringify({ Orientation: 2 }), function (data,status) {
                        var elo = self.$el.find('#Vplaceholder');
                        elo.empty();
                        elo.append(_.template($('#selresV_template').html(), {
                            selresV_id: 'selresV', resolutions: data
                        }));
                        $('#selresV').selectpicker();
                    });
                    break;
                case "#tabtpl":
                   
                    break;
            }

            
            
        },
        resetTab: function () {
            var self = this;
            self.resetForm();
            $('#wizard .tab-pane').removeClass('active').removeClass('in');
            $('#tabstart').addClass('active').addClass('in'); 
        },
        resetForm: function () {
            var self = this;
            var _model = self.model;
            _model.clear()
                  .set(_model.defaults);
                
            $('.form-error').remove();
            $('.error').removeAttr('style');
            $('.error').removeAttr('data-validation-current-error');
            $('.error').removeClass('error');

        },
        
        render: function () { 
            $.validate();
        },
        destroy: function () {
            var self = this; 
            
        },
        
        
       


    });

    var ScreenTemplateSelectorView = Backbone.View.extend(
    {
        initialize: function (options) {
            var self = this;
            self.options = options;
            self.m_screens = [];
            self.m_direction = 'left';
            self._subscribe();
        },
        render: function () {
            var self = this;
             
            $('#screen_templates').empty(); 
            var resolution = self.options.resolution;
            var orientation = self.options.orientation;

            for (var screenType in ScreenTemplate[orientation][resolution]) {

                var screenTemplateData = {
                    orientation: orientation,
                    resolution: resolution,
                    screenType: screenType,
                    screenProps: ScreenTemplate[orientation][resolution][screenType],
                    scale: 14
                };

                var screenTemplate = new ScreenTemplateFactory(
                {
                    i_screenTemplateData: screenTemplateData,
                    i_type: Backbone.CONSTS.ENTIRE_SELECTABLE,
                    i_owner: self
                });

                var snippet = screenTemplate.create();
                $('#screen_templates').append($(snippet));
                screenTemplate.activate();
                self.m_screens.push(screenTemplate);
            }
        },
        _subscribe: function () {
            var self = this;
            amplify.subscribe(Backbone.EVENTS.ON_SCREEN_TEMPLATE_SELECTED, function screenTemplateSelected(obj)
            {
                var model = self.options.wizard.model;
                var oJson = model.attributes;
                oJson.ScreenTemplate = obj;

                amplify.request(Backbone.REQUEST.ADD_CAMPAIGN, JSON.stringify(oJson), function (data, status) {

                    var oJson = {};
                    oJson.Action = 'EditCampaign';
                    oJson.DateTimeRequest = moment().format();
                    oJson.DataTarget = data.Campaign;
                    _Post(document.URL, oJson);
                  
                });


                amplify.unsubscribe(Backbone.EVENTS.ON_SCREEN_TEMPLATE_SELECTED, screenTemplateSelected);
            });
        },
    });


</script>

<script type="text/javascript">
    /* ready */
    $(function ()
    {
        var campaignsMenuView = new CampaignsMenuView({
            el: $('#campaigns_menu')
        });

        campaignsMenuView.render();

        $('.editcampaign').on('click', function (e) {
            
            var uid = $(e.target).data('uid');
            var oJson = {};
            oJson.Action = 'EditCampaign';
            oJson.DateTimeRequest = moment().format();
            oJson.DataTarget = uid;
            _Post(document.URL, oJson);
            campaignsMenuView.destroy();

             
        });

        
    });

</script>


<script id="selresV_template" type="text/html">
   <select id="<@= selresV_id @>" class="selectpicker" data-style="btn-primary" data-width="auto" title="Select Resolution">
        <option value="0">Select Resolution</option>
        <@ _.each(resolutions,function(o,e) { @>
        <option value="<@= o @>"><@=o@></option>
        <@ });@> 
    </select>
</script>

<script id="selresH_template" type="text/html">
    <select id="<@= selresH_id @>" class="selectpicker" data-style="btn-primary" data-width="auto" title="Select Resolution">
        <option value="0">Select Resolution</option>
        <@ _.each(resolutions,function(o,e) { @>
        <option value="<@= o @>"><@=o@></option>
        <@ });@> 
    </select>
</script>

