using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class PrivilegeDef
    {
        // Module Access
        public const string AccessParameters = "1000";
        public const string AccessJobsModule = "1001";
        public const string AccessRevenuesNCollection = "1002";
        public const string AccessTrafficReports = "1003";
        public const string AccessStaffObservation = "1004";
        public const string AccessAlarmsModule = "1005";
        public const string AccessDataManagement = "1006";
        public const string AccessTnGMonitoring = "1007";
        public const string AccessMHAReports = "1008";

        public const string ManageGeneralParameter = "1009";
        public const string ManageStaffParameter = "1010";
        public const string ManageSoftwareParameter = "1011";
        public const string ManageAuthenticationParameter = "1012";
        public const string ManageCardManagementParameter = "1013";


        public const string SendParameter = "1014";
        public const string ViewParameterRevisionHistory = "1015";
        public const string ViewSoftwareVersionHistory = "1016";
        public const string ManageListOfSession = "1017";
        public const string ManageListOfJobs = "1018";
        public const string ManageIndividualTransaction = "1019";
        public const string ManageOfflineData = "1020";
        public const string ManageOfflineParameter = "1021";
        public const string ManageBackupNRestore = "1022";
        public const string ManageTnGoMonitoring = "1023";
        public const string ManageMHAModule = "1024";
        public const string GenerateSessionReports = "1025";
        public const string GenerateCashCollectionReports = "1026";
        public const string GenerateEndOfShiftReports = "1027";
        public const string GenerateCloseBankInReports = "1028";
        public const string GenerateIndividualTransactionsReports = "1029";
        public const string GenerateCashReconciliationReports = "1030";
        public const string GenerateRevenueCollectionReports = "1031";
        public const string GenerateTrafficReports = "1032";
        public const string GenerateAlarmsReports = "1033";
        public const string GenerateStaffObservationReports = "1034";
        public const string GenerateDataManagementReports = "1035";
        public const string GenerateTnGoMonitoringReports = "1036";
        public const string GenerateMHAReports = "1037";
        public const string LoginHQA = "1038";
        public const string LoginSCW = "1039";
        public const string LoginToLCS = "7000";
        public const string StartNormalJobNormalMode = "7101";
        public const string StartNormalJobExpressMode = "7102";
        public const string StartNormalJobDedicatedMode = "7103";
        public const string StartMaintenanceJobNormalMode = "7201";
        public const string StartMaintenanceJobExpressMode = "7202";
        public const string StartMaintenanceJobDedicatedMode = "7203";
        public const string StartMaintenanceJobPeripheralTest = "7204";
        public const string StartMaintenanceJobManualUpload = "7205";
        public const string StartMaintenanceJobManualDownload = "7206";
        public const string LoginToTOD = "2000";
        public const string RegisterAttendance = "2100";
        public const string DeclareCollection = "2200";
        public const string DeclareCashCollection = "2201";
        public const string DeclareCashETCCollection = "2202";
        public const string RegisterHandheldTerminal = "2300";
        public const string UnregisterHandheldTerminal = "2302";
        public const string Messaging = "2400";
        public const string ComposeMessage = "2401";
        public const string ReadMessage = "2402";
        public const string LoginToCMS = "";
        public const string LoginToCMT = "";
        public const string InitializeCard = "";
        public const string AccessCMSParameter = "";
        public const string ManualBlacklistCard = "";
        public const string ManagePlazaThreshold = "";
        public const string CardNMagazineInventory = "";
        public const string ViewMagazineInventoryByPlaza = "";
        public const string GenerateMagazineInventoryByPlazaReport = "";
        public const string ViewInventoryByMagazine = "";
        public const string GenerateInventoryByMagazineReport = "";
        public const string ViewInTransitCards = "";
        public const string GenerateInTransitCardsReport = "";
        public const string SubmitMagazineRequest = "";
        public const string SubmitCardsRequest = "";
        public const string CMSDataManagement = "";
        public const string ImportPlazaData = "";
        public const string ExportPlazaData = "";
        public const string StockRegisteratCMT = "";
        public const string CheckMagazineState = "";
        public const string UpdateMagazineState = "";
        public const string CheckCardHistory = "";
        public const string RegisterMagazineStockOut = "";
        public const string RegisterMagazineStockIn = "";
        public const string RegisterCardStockOut = "";
        public const string LooseCardManagement = "";
        public const string GeneratePreIssuedCards = "";
        public const string RegisterInPreIssuedCards = "";
        public const string RegisterInFoundClass = "";
        public const string RegisterInFaultyClass = "";
        public const string RegisterInAutoBlacklistCards = "";
        public const string RegisterInCMTJob = "";
        public const string RegisterInMTLJob = "";

    }
}