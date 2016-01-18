<%@ Page Title="" Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="False" CodeBehind="Accounts.aspx.cs" Inherits="HIS.Web.UI.Accounts" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="page-title">Accounts
            <small></small>
            </h2>
        </div>
    </div>
    <sym:CallbackPanel ID="ControlPanel" runat="server" ClientCallBackFunction="ucCallback" containertriggers-capacity="4">
        <asp:PlaceHolder ID="ControlPlaceHolder" runat="server"></asp:PlaceHolder>
    </sym:CallbackPanel>    
    <script type="text/javascript">

        var Accounts =
        {
            
        };

        $(function () {
           


        });

        var ucCallback = function () {
            console.log('callback!');
        };

    </script>

</asp:Content>
