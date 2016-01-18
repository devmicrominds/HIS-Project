using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class GroupsPresenter : GenericPresenter<IGroupsView>
    {
        private IRepositoryFactory factory;
        private Dictionary<string, Action> RunAction;


        public GroupsPresenter(IRepositoryFactory factory)
        {

            this.factory = factory;
            BuildRunActions();
        }

        private void BuildRunActions()
        {
            this.RunAction = new Dictionary<string, Action>() { 
               { "EditSchedule",ProcessEditSchedule },
               { "EditGroupGrid",ProcessEditGrid },
               { "ReturnGroupList",ProcessReturnGroupList },
               { "ShowTab",ProcessShowTab },
               { "SaveGroup",ProcessSaveGroup },
            };
        }

        private void ProcessSaveGroup()
        {

            var p = View.PostParameter;
            var data = p.Data;
            string idU = data.ID;
            bool delete = data.Operation;
            bool isNew = false;

            Guid groupids = idU == null ? Guid.Parse("00000000-0000-0000-0000-000000000000") : Guid.Parse(idU);

            var repository = factory.GetRepository<Repository<HIS.Data.Groups>>();
            var repositoryShedule = factory.GetRepository<Repository<HIS.Data.Schedule>>();
            var query = repository.GetQueryable();
            var oGroup = query.Where(o => o.Id == groupids).FirstOrDefault();


            if (!delete)
            {
                if (oGroup == null)
                {
                    isNew = true;
                    oGroup = new HIS.Data.Groups()
                    {
                        Name = data.Name,
                        Description = data.Description

                    };

                }
                else
                {
                    oGroup.Name = data.Name;
                    oGroup.Description = data.Description;


                }

                factory.OnTransaction(() =>
                {

                    oGroup = repository.SaveOrUpdate(oGroup);
                    if (isNew)
                    {
                        Schedule oShed = new Schedule();
                        oShed.Groups = oGroup;
                        oGroup.AddSchedule(oShed);
                    }

                });




            }
            else
            {
                factory.OnTransaction(() =>
                {

                    repository.Delete(oGroup);
                });
            }

            View.AddControl(ControlPath.IGroupsListView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }

        public void ProcessEditGrid()
        {
            View.AddControl(ControlPath.IGroupsManageView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }


        private void ProcessShowTab()
        {
            ClearPageSettings();
            string datatarget = View.PostParameter.DataTarget;
            switch (datatarget)
            {
                case "group.add":
                    View.AddControl(ControlPath.IGroupsManageView, View.ControlPlaceHolder);
                    break;
                case "group.view":
                    View.AddControl(ControlPath.IGroupsListView, View.ControlPlaceHolder);
                    break;
            }

            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }
        public override void InitialRun()
        {
            base.InitialRun();
            View.AddControl(ControlPath.IGroupsListView, View.ControlPlaceHolder);

        }

        public override void PartialRender()
        {
            string action = View.PostParameter.Action;
            RunAction[action]();

        }

        public void ProcessEditSchedule()
        {
            View.AddControl(ControlPath.IGroupsScheduleView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder, new[] { "EditSchedule" });
        }

        public void ProcessReturnGroupList()
        {
            View.AddControl(ControlPath.IGroupsListView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }

        
    }
}