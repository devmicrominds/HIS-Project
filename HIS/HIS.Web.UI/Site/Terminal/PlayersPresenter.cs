using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class PlayersPresenter : GenericPresenter<IPlayersView>
    {
        private IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;



        public PlayersPresenter(IRepositoryFactory factory)
        {
            this.factory = factory;
            BuildRunActions();

        }


        private void BuildRunActions()
        {
            RunAction = new Dictionary<string, Action>() 
            {
                 { "EditPlayer",ProcessEditSave },
                 { "ShowTab",ProcessShowTab },
                 //{ "ReturnGroupList",ProcessReturnGroupList },
                 { "EditPlayerGrid",EditGrid },
                 
            };
        }



        public override void InitialRun()
        {
            base.InitialRun();
            View.AddControl(ControlPath.IPlayersListView, View.ControlPlaceHolder);

        }
        public override void PartialRender()
        {
            string action = View.PostParameter.Action;
            RunAction[action]();

        }

        public void ProcessEditSchedule()
        {
            var p = View.PostParameter;
            var data = p.Data;

            string a = data.Name;
            string b = data.ID;




            View.AddControl(ControlPath.IPlayersSettings, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder, new[] { "EditSchedule" });
        }

        public void EditGrid()
        {
            View.AddControl(ControlPath.IPlayersEditView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }


        //public void ProcessReturnGroupList()
        //{
        //    View.AddControl(ControlPath.IPlayersListView, View.ControlPlaceHolder);
        //    View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        //}

        private void ProcessEditSave()
        {
            var p = View.PostParameter;
            var data = p.Data;
            string idG = data.GroupID;
            string idP = data.PlayerID;
            bool delete = data.Operation;

            /* Guid groupids = Guid.Parse(idG);*/
            Guid groupids = string.IsNullOrEmpty(idG) ? Guid.Parse("00000000-0000-0000-0000-000000000000") : Guid.Parse(idG);

            Guid playerids = idP == null ? Guid.Parse("00000000-0000-0000-0000-000000000000") : Guid.Parse(idP);

            var repository = factory.GetRepository<Repository<Player>>();
            var query = repository.GetQueryable();

            var oPlayer = query.Where(o => o.Id == playerids).FirstOrDefault();

            if (!delete)
            {
                if (oPlayer == null)
                {
                    oPlayer = new Player()
                   {
                       Name = data.Name,
                       Location = data.Location,
                       Groups = GetGroupsRoleById(groupids),
                   };
                }
                else
                {
                    oPlayer.Name = data.Name;
                    oPlayer.Location = data.Location;
                    oPlayer.Groups = GetGroupsRoleById(groupids);


                }
                factory.OnTransaction(() =>
                {
                    oPlayer = repository.SaveOrUpdate(oPlayer);
                });
            }
            else
            {
                /*kantoi*/
                factory.OnTransaction(() =>
                {
                    repository.Delete(oPlayer);
                });
            }

            View.AddControl(ControlPath.IPlayersListView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);

            #region OLD

            //if (isExist == null)
            //{
            //    var oUser = new Player()
            //    {
            //        Name = data.Name,
            //        Location = data.Location,
            //        Groups = GetGroupsRoleById(groupids),
            //    };


            //    factory.OnTransaction(() =>
            //    {
            //        oUser = repository.SaveOrUpdate(oUser);
            //    });
            //}
            //else
            //{
            //    player.Name = data.Name;
            //    player.Location = data.Location;
            //    player.Groups = GetGroupsRoleById(groupids);
            //    var oUser = player;


            //    factory.OnTransaction(() =>
            //    {
            //        oUser = repository.SaveOrUpdate(oUser);
            //    });
            //}
            #endregion

        }
        //private void ProcessSaveUser()
        //{
        //    ProcessEditSave();
        //    View.AddControl(ControlPath.IPlayersListView, View.ControlPlaceHolder);
        //    View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        //}

        //private void ProcessEditUser()
        //{
        //    ProcessEditSave();
        //    View.AddControl(ControlPath.IPlayersListView, View.ControlPlaceHolder);
        //    View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        //}

        private Groups GetGroupsRoleById(Guid guid)
        {

            var repository = factory.GetRepository<Repository<Groups>>();
            var query = repository.GetQueryable();
            return query.Where(o => o.Id == guid).FirstOrDefault();

        }


        private void ProcessShowTab()
        {
            ClearPageSettings();
            string datatarget = View.PostParameter.DataTarget;
            switch (datatarget)
            {
                case "player_view":
                    View.AddControl(ControlPath.IPlayersListView, View.ControlPlaceHolder);
                    break;
                case "player_add":
                    View.AddControl(ControlPath.IPlayersEditView, View.ControlPlaceHolder);
                    break;
            }

            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
        }

    }
}