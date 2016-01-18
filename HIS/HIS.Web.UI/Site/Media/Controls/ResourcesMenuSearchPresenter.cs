using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class ResourcesMenuSearchPresenter : GenericPresenter<IResourcesMenuSearchView>
    {
        IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;

        public ResourcesMenuSearchPresenter(IRepositoryFactory factory)
        {

            this.factory = factory;
            BuildRunActions();
        }
        public override void View_Load(object sender, EventArgs e)
        {
            var parameter = View.PostParameter;
            if (parameter != null)
            {
                string action = parameter.Action;

                if (RunAction.ContainsKey(action))
                {
                    RunAction[action]();
                }
            }

        }

        private void BuildRunActions()
        {
            RunAction = new Dictionary<string, Action>() 
            {
                 
              
                 { "NavigateTo",NavigateTo },
                 
            };
        }

        private void NavigateTo()
        {

            string target = View.PostParameter.Data.Context;
            string key = View.PostParameter.KeyInput;
            switch (target)
            {
                case "ClickSearch":
                    ProcessGetDataDummy(key);
                    /*ProcessGetData(key);*/
                    break;
            }
        }

        public void ProcessGetDataDummy(string key)
        {




            View.JsonData = Json(
                new
                {
                    Source = "return",
                    InputKey = key,


                }
            );

        }




        /*public void ProcessGetData(string key)
        {
            testGetData(key);

            var searchData = new
            {
                Data01 = new
                {
                    ID = 1,
                    SEQNO = 1,
                    MediaType = "Video",
                    File = new { ID = "movie1", Name = "MovieName" },
                },
                Data02 = new
                {
                    ID = 2,
                    SEQNO = 2,
                    MediaType = "Music",
                    File = new { ID = "music01", Name = "MusicName" },
                }
                ,
                Data03 = new
                {
                    ID = 3,
                    SEQNO = 3,
                    MediaType = "Image",
                    File = new { ID = "Image01", Name = "ImageName" },
                },



            };


            View.JsonData = Json(
                new
                {
                    DataFromDB = searchData,
                    InputKey = key,


                }
            );

        }*/

        public void ProcessGetData(string key)
        {

            var repository = factory.GetRepository<Repository<Resource>>();
            var query = repository.GetQueryable();

            var list = query.Where(o => o.Name.Contains(key))
                ;

            var result = list.Select((o, i) =>
                  new
                 {
                     Id = o.Id,
                     SeqNo = i.ToString(),
                     MediaType = o.ResourceType == ResourceType.Video ? "Video" : o.ResourceType == ResourceType.Image ? "Image" : "Music",
                     Name = o.Name
                 }
            );



            View.JsonData = Json(
                new
                {
                    DataFromDB = result,
                    InputKey = key,


                }
            );

        }




    }
}