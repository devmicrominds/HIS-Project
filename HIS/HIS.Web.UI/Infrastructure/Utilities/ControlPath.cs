using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class ControlPath
    {
        public const string FilterDirectory = "Filters/";
        public const string ControlDirectory = "Controls/";
        public const string ascx = ".ascx";
        // Account
        public const string IAccountsAddEditView = ControlDirectory + "Accounts_AddEdit" + ascx;
        public const string IAccountsManageView = ControlDirectory + "Accounts_ManageRoles" + ascx;
        public const string IAccountsPrivilegesView = ControlDirectory + "Accounts_Privileges" + ascx;
        public const string IAccountsRolesView = ControlDirectory + "Accounts_Roles" + ascx;
        public const string IAccountsUsersView = ControlDirectory + "Accounts_Users" + ascx;
        public const string IAccountsUsersListView = ControlDirectory + "Accounts_Users_List" + ascx;
        public const string IGroupsListView = ControlDirectory + "Groups_List" + ascx;
        public const string IGroupsManageView = ControlDirectory + "Group_Manage" + ascx;
        public const string IGroupsScheduleView = ControlDirectory + "Groups_Schedule" + ascx;

        public const string ICampaignsListView = ControlDirectory + "Campaigns_List" + ascx;
        public const string ICampaignsEditorView = ControlDirectory + "Campaigns_Editor" + ascx;
        public const string ICampaignsMediaSelectorView = ControlDirectory + "Campaigns_Media_Selector" + ascx;
        public const string ICampaignsLayoutSelectorView = ControlDirectory + "Campaigns_Layout_Selector" + ascx;
        public const string ICampaignsMediaSelectorAltView = ControlDirectory + "Campaigns_Media_Selector_Alt" + ascx;

        public const string IResourcesMenuView = ControlDirectory + "Resources_Menu" + ascx;
        public const string IResourcesMenuSearchView = ControlDirectory + "Resources_Menu_Search" + ascx;
        public const string IResourcesImageView = ControlDirectory + "Resources_Image" + ascx;
        public const string IResourcesVideoView = ControlDirectory + "Resources_Video" + ascx;
        public const string IResourcesHTML5View = ControlDirectory + "Resources_HTML5" + ascx;
        public const string IResourcesStreamView = ControlDirectory + "Resources_Stream" + ascx;
        public const string IResourcesMusicView = ControlDirectory + "Resources_Music" + ascx;

        public const string IResourcesSearchGalleryView = ControlDirectory + "Resources_Search_Gallery" + ascx;

        public const string IResourcesFileUploadView = ControlDirectory + "Resources_FileUpload" + ascx;
        public const string IResourcesMediaGalleryView = ControlDirectory + "Resources_Media_Gallery" + ascx;

        public const string IPlayersListView = ControlDirectory + "Players_List" + ascx;
        public const string IPlayersSettings = ControlDirectory + "Players_Settings" + ascx;
        public const string IPlayersEditView = ControlDirectory + "PlayerEdit" + ascx;

        public const string IResource_TickerView = ControlDirectory + "Resource_Ticker" + ascx;
        public const string IResourcesHTMLAddEditView = ControlDirectory + "Resources_HTML_AddEdit" + ascx;
        public const string IResourceCCTVView = ControlDirectory + "Resource_CCTV" + ascx;

        public const string ITestjs01Settings = ControlDirectory + "UCTestjs01_1" + ascx;
        public const string IJsonTest = ControlDirectory + "Ucjson" + ascx;
  
    }
}