<%@ Page Title="" Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="true" CodeBehind="Groups.aspx.cs" Inherits="HIS.Web.UI.Site.Terminal.Groups" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/Contents/lib/fullcalendar/fullcalendar.min.css" rel="stylesheet" />

    <div class="row">
        <div class="col-md-12">
            <h2 class="page-title">Groups
            <small></small>
            </h2>
        </div>
    </div>
    <sym:CallbackPanel ID="ControlPanel" runat="server" ClientCallBackFunction="__groups.callbackfunc" containertriggers-capacity="4">
        <asp:PlaceHolder ID="ControlPlaceHolder" runat="server"></asp:PlaceHolder>
    </sym:CallbackPanel>
    <script type="text/javascript" src="/Contents/lib/fullcalendar/fullcalendar.min.js"></script>
    <script type="text/javascript">

        var __groups =
        {
            initialize: function () {

            },
            callbackfunc: function () {
                var args = __groups.callbackfunc.arguments;
                switch (args[0]) {
                    case "EditSchedule":
                        __groups.viewschedule();
                        break;
                    default:
                        break;
                }
            },
            viewschedule: function () {
                $('#tablist a[href="#tab_schedule"]').tab('show')
            },
            viewgrouplist: function () {
                $('#tablist a[href="#tab_groups"]').tab('show')
            }


        };

        $(function () {
            __groups.initialize();

        });





    </script>
</asp:Content>
