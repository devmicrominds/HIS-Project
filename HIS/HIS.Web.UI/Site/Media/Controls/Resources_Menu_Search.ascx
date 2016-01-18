<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Resources_Menu_Search.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_Menu_Search" %>

<div class="row">
    <div class="col-md-10">
        <section class="widget">


            <div class="tab-content">
                <div id="tab_view" class="tab-pane fade active in">


                    <fieldset>
                        <legend style="padding-left: 10px;">Search Media</legend>

                    </fieldset>

                    <div class="control-group">
                        <div class="controls">
                            <input id="i_search" data-action="get_search" class="search col-md-10" placeholder="Search ...." type="text" maxlength="25" />
                        </div>



                        <div id="listView">
                        </div>


                    </div>
                </div>

            </div>
        </section>
    </div>
</div>

<script type="text/javascript">

    var ResourcesMenuTemplate = __jsonModel;
    var ResourcesMenuSearch = Backbone.View.extend(
{
    initialize: function () {
        var self = this;

        $('#i_search').keyup(function (e) {
            self.trigger('enter');

        });
        self.m_model = {};

        /*var snippet = _.template($('#search_template001').html(),
         {
             resources: self.model.DataFromDB,

         });

        $('#listView').append(snippet);
        $("#i_search").val(self.model.InputKey);*/



    },
    _render: function () {
        var self = this;
        /*This Code use to bind again when return from other form*/
        if (self.model.Source == 'return') {
            $("#i_search").val(self.model.InputKey);
            this._getSearchData();
        }
    },
    events:
            {

                'click button[data-action=preview_Content]': '_preview_Content',
                'keyup :input': 'submitHandler',

            },

    _getSearchData: function () {

        $.ajax
  (
      {
          url: '/app/Media/GetSearchMedia?key=' + $("#i_search").val(),
          type: 'GET',
          datatype: 'json',
          success: function (data) {

              self.m_model = data;
              self.model = data;

              _.each(self.m_model, function (o, e) {
                  o.KeyInput = $("#i_search").val();
              });

              var snippet = _.template($('#search_template001').html(),
               {
                   resources: data,

               });


              $('#listView').empty();
              $('#listView').append(snippet);

          }

      }
  );
        /*  if (!$("#i_search").val())
              alert("Please Insert Key");
          else {
              var data = {
                  Action: 'NavigateTo',
                  Data: { Context: 'ClickSearch' },
                  KeyInput: $("#i_search").val(),
              };
              amplify.publish('RESOURCE_MENU_ACTION_REQUEST', data);
          }*/



    },
    _preview_Content: function (e) {
        var self = this;
        var cmd = $(e.target);
        var datatarget = cmd.data('uid');

        /*var mediaType = '';
        var name;
        console.log("PREVIEW");
        console.log(self.m_model);
        console.log(self.model);
        console.log(ResourcesMenuTemplate);


        mediaType = 'Video';
        name = 'Iklan01';
        _.each(self.m_model, function (o, e) {

            if (o.Id == datatarget) {
                mediaType = o.MediaType;
                name = o.Name;
            }
        });
        */

        var data = {
            Action: 'NavigateTo',
            Data: { Context: 'SelectedItemSearch' },
            DataTarget: datatarget,
            /*MediaType: mediaType,
            Name: name,*/
            KeyInput: $("#i_search").val(),
        };
        amplify.publish('RESOURCE_MENU_ACTION_REQUEST', data);


    },
    submitHandler: function () {



        var self = this;

        if (self.timer)
            clearTimeout(self.timer);
        self.timer = setTimeout(function () {
            self._getSearchData();
            self.timer = null;
        }, 500);
    },

});



    $(document).ready(function () {

        __JsonModel(<%= this.JsonData %>);



        var ResourcesMenuTemplate = __jsonModel;

        var resourceMenuSearch = new ResourcesMenuSearch({
            el: "#tab_view",
            model: __jsonModel,
        });

        resourceMenuSearch._render();


    });


</script>






<script id="search_template001" type="text/html">
    <div class="row">
        <table class="table table-striped table-images">
            <thead>
                <tr>
                    <th class="hidden-xs-portrait">No</th>
                    <th>Media Type</th>
                    <th>Name</th>
                    <th>Show</th>
                </tr>
            </thead>
            <tbody>
                <@ _.each(resources,function(o,e){  @>
                
             
           
               
                <tr>
                    <td><@= o.SeqNo @></td>
                    <td><@= o.MediaType @></td>
                    <td><@= o.Name @></td>
                    <td>
                        <button data-uid="<@= o.Id @>" type="button" data-action="preview_Content" class="editplayer btn btn-sm btn-info"><i class="fa fa-laptop"></i></button>
                    </td>
                </tr>
                <@ }); @>
            </tbody>
        </table>
    </div>
</script>
