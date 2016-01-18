<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Players_Settings.ascx.cs" Inherits="HIS.Web.UI.Site.Terminal.Controls.Players_Settings" %>
<div class="row">
    <div class="col-md-10">
        <section class="widget">
            <div class="pull-right">
                <button id="btnReturn" type="button" class="btn btn-info">Return</button>
            </div>
        </section>
    </div>
</div>
<script type="text/javascript">


    $(function () {

        $('#btnReturn').on('click', function () {

            var json = {
                Action: 'ReturnGroupList',
            };

            _Post(document.URL, json);



        });

    });

</script>
