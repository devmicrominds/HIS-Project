<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Resources_FileUpload.ascx.cs" Inherits="HIS.Web.UI.Site.Media.Controls.Resources_FileUpload" %>
<div id="resource_fileupload">
    <div id="resource_fileupload_pageheader" class="header"></div>
    <div id="fileupload">
        <div class="row">
            <div class="col-md-12">
                <div id="dropzone" class="dropzone">
                    <i class="fa fa-download-alt pull-right"></i>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">

    Dropzone.autoDiscover = false;

    var ResourcesFileUpload = Backbone.View.extend(
    {
        initialize: function (options) {
            var self = this;

        },
        render: function ()
        {
            var self = this;
            var header_title = self.model.Data.Context;
            self._updatePageHeader(header_title);
            self._setupDropzone();
            console.log('update page header completed');
        },
        _updatePageHeader: function (header_title) {
            var self = this;
            var pageheader = this.$('#resource_fileupload_pageheader');
            pageheader.empty();
            var snippet = _.template($('#resource_fileupload_header_template').html(), {
                headertitle:header_title
            });
            pageheader.html(snippet);
             
        },
        _setupDropzone: function () {
            var self = this;
             
            $('.dz-hidden-input').remove();
            var zone = this.$el.find('#dropzone');
            zone.dropzone(
            {
                url: '/app/media/uploadfile',
                uploadMultiple: true,
                maxFilesize: '2048',
                addRemoveLinks: true,
                acceptedFiles: self.model.Data.AcceptedFiles,
                init: function () {

                    this.on('sendingmultiple', function (file, xhr, formData) {
                        formData.append('UploadModel', JSON.stringify(self.model));
                    });

                    this.on('completemultiple', function (file) {

                        console.log(file);
                    });
                }
            });

           
        }
    });

    $(function () {

        console.log('aaa');
        __JsonModel(<%= this.JsonData %>);
        console.log('ooo');
        var view = new ResourcesFileUpload(
        {
            el: '#resource_fileupload',
            model: __jsonModel
        });

        view.render();

    });

</script>

<script id="resource_fileupload_header_template" type="text/html">
    <header>
        <h4><@= headertitle @> Upload</h4>
    </header>
</script>
