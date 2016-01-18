<%@ Page Title="" Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="false" CodeBehind="Players.aspx.cs" Inherits="HIS.Web.UI.Site.Terminal.Players" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="page-title">Players
            <small></small>
            </h2>
        </div>
    </div>
    <sym:callbackpanel id="ControlPanel" runat="server" clientcallbackfunction="ucCallback" containertriggers-capacity="4">
        <asp:PlaceHolder ID="ControlPlaceHolder" runat="server"></asp:PlaceHolder>
    </sym:callbackpanel>

    <script type="text/javascript">

        /* var __players =
        {
            initialize: function () {

            },
            callbackfunc: function () {
             
              
            },
            viewschedule: function () {
               
            },
            viewgrouplist: function () {
                $('#tablist a[href="#tab_groups"]').tab('show')
            }


        };

        $(function () {
            __players.initialize();

        });*/
        var ucCallback = function () {
            console.log('callback!');
        };





    </script>
</asp:Content>
