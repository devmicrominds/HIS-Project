using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class CampaignsMediaSelectorPresenter  : GenericPresenter<ICampaignsMediaSelectorView>
    {
        private IRepositoryFactory factory;

        public CampaignsMediaSelectorPresenter(IRepositoryFactory factory)
        {
            this.factory = factory;
        }
        public override void View_Load(object sender, EventArgs e) {

            var input = View.PostParameter;
            string action = input.Action;
            switch (action) {
                case "SelectMedia":
                    SelectMedia();
                    break;
            
            }
            
        }

        public void SelectMedia ()  {

            var input = View.PostParameter.Data;
            string context = input.Context;
            Guid channelId = input.Channel;

            dynamic result = null;

            switch (context)
            { 
                case "Image":
                    result = GetMedia(ResourceType.Image);
                    break;
                case "Video":
                    result = GetMedia(ResourceType.Video);
                    break;
                case "HTML5":
                    result = GetMedia(ResourceType.HTML);
                    break;
                case "Stream":
                    result = GetMedia(ResourceType.Stream);
                    break;
                case "Music":
                    result = GetMedia(ResourceType.Music);
                    break;
                default:
                    break;
            }


            View.JsonData = Json(new {
                Context = context,
                Channel = channelId,
                Media = result,
            });
        }

        public dynamic GetMedia(ResourceType resourceType) 
        {

            var repository = factory.GetRepository<Repository<MediaCategory>>();
            
            var query = repository.GetQueryable();
            
            var result = query.Where(o => o.MediaResourceType == resourceType)
                         .Select(o=> new  
                         {
                             Id= o.Id,
                             Item = 
                             new {
                                Name =  o.Name,
                                Count = o.Resources.Count
                             }

                         }).ToDictionary(prop=>prop.Id, prop=>prop.Item);

            return result;

             
        }
    }
}