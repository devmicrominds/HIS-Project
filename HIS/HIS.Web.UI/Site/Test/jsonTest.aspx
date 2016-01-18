<%@ Page Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="true" CodeBehind="jsonTest.aspx.cs" Inherits="HIS.Web.UI.Site.Test.jsonTest" %>

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
        console.log(__jsonModel);
    </script>
</asp:Content>
