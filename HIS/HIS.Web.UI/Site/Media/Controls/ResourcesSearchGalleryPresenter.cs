using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.Media.Controls
{

    public class ResourcesSearchGalleryPresenter : GenericPresenter<IResourcesSearchGalleryView>
    {

        IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;

        public ResourcesSearchGalleryPresenter(IRepositoryFactory factory)
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
            switch (target)
            {
                case "SelectedItemSearch":
                    ProcessGetData();
                    break;
            }
        }

        public void ProcessGetData()
        {
            string target = View.PostParameter.Data.Context;
            string key = View.PostParameter.KeyInput;
            string datatarget = View.PostParameter.DataTarget;
            Guid guid = new Guid(datatarget);
            /*string mediaType = View.PostParameter.MediaType;
            string name = View.PostParameter.Name;*/

            //this code from resouce

            var repositoryMain = factory.GetRepository<Repository<HIS.Data.Resource>>();
            var query = repositoryMain.GetQueryable();
            var result = query.Where(x => x.Id == guid).SingleOrDefault();

            switch (result.ResourceType)
            {
                case ResourceType.Video:
                    var repository = factory.GetRepository<Repository<HIS.Data.ResourceVideo>>();
                    var queryVideo = repository.GetQueryable();
                    queryVideo = queryVideo.Where(x => x.Id == guid);
                    var resultVideo = queryVideo.Select(o => new
                     {
                         Name = result.Name,
                         Id = datatarget,
                         FileName = o.Filename,
                         Size = o.Size
                     });
                    View.JsonData = Json(new
                    {
                        SelectedData = resultVideo,
                        KeyInput = key,
                        MediaType = "Video",


                    });
                    break;
                case ResourceType.Music:
                    var repMusic = factory.GetRepository<Repository<HIS.Data.ResourceMusic>>();
                    var qMusic = repMusic.GetQueryable();
                    qMusic = qMusic.Where(x => x.Id == guid);
                    var resultMusic = qMusic.Select(o => new
                    {
                        Name = result.Name,
                        Id = datatarget,
                        FileName = o.Filename,
                        Size = o.Size
                    });
                    View.JsonData = Json(new
                    {
                        SelectedData = resultMusic,
                        KeyInput = key,
                        MediaType = "Music",

                    });
                    break;
                case ResourceType.Image:
                    var repImage = factory.GetRepository<Repository<HIS.Data.ResourceImage>>();
                    var qImage = repImage.GetQueryable();
                    qImage = qImage.Where(x => x.Id == guid);
                    var resultImage = qImage.Select(o => new
                    {
                        Name = result.Name,
                        Id = datatarget,
                        FileName = o.Filename,
                        Size = o.Size
                    });
                    View.JsonData = Json(new
                    {
                        SelectedData = resultImage,
                        KeyInput = key,
                        MediaType = "Image",

                    });
                    break;
            }


        }
    }


}