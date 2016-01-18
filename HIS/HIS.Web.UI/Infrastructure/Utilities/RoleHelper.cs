using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{

    public class RoleHelper
    {
     
        private Dictionary<string, string> directoryRoles = new Dictionary<string, string>();

        private Dictionary<string, string> fileRoles = new Dictionary<string, string>();

        private Dictionary<string, string> roleDesc = new Dictionary<string, string>();
        
        private List<string> dirOrder = new List<string>();

        private List<string> fileOrder = new List<string>();

        public Dictionary<string, string> RoleDescription {

            get {
                return roleDesc;
            }
        }

        public Dictionary<string, string> DirectoryRoles {
            
            get {
                return directoryRoles; 
            }
        }

        public Dictionary<string, string> FileRoles { 
        
            get {
                return fileRoles; 
            }
        }

        public List<string> DirOrder { 
            get {
                return dirOrder; 
            } 
        }

        public List<string> FileOrder { 
        
            get {
                return fileOrder; 
            }
        }

        public RoleHelper()
        {
            Build();
        }

        private void Build()
        {

            PopulateFolderToRole();
            PopulateFileToRole();
            PopulateDirOrder();
            PopulateFileOrder();
            PopulateRoleDescription();
        }

        private void PopulateRoleDescription()
        {
            roleDesc[DirConst.Administration] = "edit";
            roleDesc[DirConst.Terminal] = "desktop";
            roleDesc[DirConst.Media] = "film";
            roleDesc[DirConst.Reporting] = "columns";
            
        }

        private void PopulateFolderToRole()
        {
            directoryRoles[DirConst.Administration] = PrivilegeDef.LoginHQA;
            directoryRoles[DirConst.Terminal] = PrivilegeDef.LoginHQA;
            directoryRoles[DirConst.Media] = PrivilegeDef.LoginHQA;
            directoryRoles[DirConst.Reporting] = PrivilegeDef.LoginHQA;
           

        }

        private void PopulateDirOrder()
        {

            dirOrder.AddRange(new[] 
            {
				DirConst.Administration,
                DirConst.Terminal,
                DirConst.Media,
                DirConst.Reporting
			});

        }

        private void PopulateFileToRole()
        {
            fileRoles[FileConst.AdministrationAccounts] = PrivilegeDef.LoginHQA;
            fileRoles[FileConst.Profile] = PrivilegeDef.LoginHQA;
            fileRoles[FileConst.Accounts02] = PrivilegeDef.LoginHQA;
            fileRoles[FileConst.TerminalGroups] = PrivilegeDef.LoginHQA;
            fileRoles[FileConst.TerminalPlayers] = PrivilegeDef.LoginHQA;
            fileRoles[FileConst.MediaCampaigns] = PrivilegeDef.LoginHQA;
            fileRoles[FileConst.MediaResources] = PrivilegeDef.LoginHQA;
            fileRoles[FileConst.ReportingActivityLogs] = PrivilegeDef.LoginHQA;

            // temp                       
        }

        private void PopulateFileOrder()
        {

            //fileOrder.AddRange(new[] { 
				
				 
				
            //});

        }
    }
}