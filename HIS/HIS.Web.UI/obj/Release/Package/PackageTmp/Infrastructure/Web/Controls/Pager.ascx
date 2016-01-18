<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="HIS.Web.UI.Pager" %>
<div class="row" style="font-family:  Candara; font-size: 12px;">
    <span class="col-lg-8">
        <span class="label label-primary" style="margin: 2px 2px 2px 2px; font-size: inherit;">
            <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
             <asp:Label runat="server" Text="# Page"></asp:Label>
            <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
            <asp:DropDownList ID="DropDownPageSize" runat="server" data-style="btn-info btn-sm" Width="85px" CssClass="pagesize selectpicker" onchange="return false;">
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
            </asp:DropDownList>
           
        </span>
    </span>
    <span class="col-lg-4">
        <span class="label label-primary" style="margin: 2px 2px 2px 16px;">
            <a id="prev" href="#" class="pageprev  btn btn-info  btn-sm"> 
                <span class="glyphicon glyphicon-chevron-left" title="Previous"></span>
            </a>
            <span>&nbsp;&nbsp;</span>
            <asp:DropDownList ID="DropDownPageIndex" data-style="btn-info  btn-sm" runat="server" onchange="return false;"
                Width="120px" CssClass="pageindex selectpicker">
            </asp:DropDownList>
            <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
            <span>of</span>
            <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
            <asp:Label ID="LabelNumberOfPages" CssClass="numberofpages" runat="server" />
            <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
            <a id="next" href="#" class="pagenext btn btn-info  btn-sm" title="Next">
                <span class="glyphicon glyphicon-chevron-right"></span>
            </a>
        </span>
    </span>


</div>

<script type="text/javascript">

    $('.selectpicker').selectpicker();

        var _virtualPath = '<%# VirtualPath %>';
        var _pagerContext = '<%# PagerContext %>'; 

        $('.pageindex').change(
            function () {
                var pageIndex = $(this).prop('value');
                var pageSize = $('.pagesize').prop('value');

                pageSettingsChanged(pageIndex, pageSize);

                return false;
            }
         );

        $('.pagesize').change(
             function () {
                 var pageIndex = 0;
                 var pageSize = $(this).prop('value');
                 pageSettingsChanged(pageIndex, pageSize);

                 return false;
             }

        );

        $('.pageprev').click(function () {
            var pageIndex = $('.pageindex').prop('value');
            var pageSize = $('.pagesize').prop('value');

            if (pageIndex > 0)
                pageIndex = pageIndex - 1;

            pageSettingsChanged(pageIndex, pageSize);

            return false;

        });


        $('.pagenext').click(function () {
            var pageIndex = $('.pageindex').prop('value');
            var pageSize = $('.pagesize').prop('value');
            var numberofpages = $('.numberofpages').html();

            if (parseInt(pageIndex) < (parseInt(numberofpages) - 1)) {
                pageIndex = parseInt(pageIndex) + 1;
            }
            else {
                pageIndex = numberofpages - 1;
            }

            pageSettingsChanged(pageIndex, pageSize);
            return false;
        });

        function pageSettingsChanged(pageIndex, pageSize) {
            var pageSettings = {};
            pageSettings.Action = 'PageSettingsChanged';
            pageSettings.VirtualPath = _virtualPath;
            pageSettings.PagerContext = _pagerContext;
            pageSettings.PageIndex = pageIndex;
            pageSettings.PageSize = pageSize;
            _Post(document.URL, pageSettings);
        };

       
     

   

</script>
