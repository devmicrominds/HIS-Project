<%@ Page Title="" Language="C#" MasterPageFile="~/Site/Base.Master" AutoEventWireup="false" CodeBehind="Resources.aspx.cs" Inherits="HIS.Web.UI.Site.Media.Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="page-title">Resources
            <small></small>
            </h2>
        </div>
    </div>
    <sym:CallbackPanel ID="ControlPanel" runat="server" ClientCallBackFunction="Resources.Callback" containertriggers-capacity="4">
        <asp:PlaceHolder ID="ControlPlaceHolder" runat="server"></asp:PlaceHolder>
    </sym:CallbackPanel>

    <script type="text/javascript">
       
        var Resources = 
        {
            Callback: function (e) {
                console.log('e  ' + e);
                if (!_.isUndefined(e))
                    alert('undefined');
                
            },
        };

        

        $(function ()
        {


        });
        
    </script>
</asp:Content>
