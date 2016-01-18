using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class CampaignsEditorPresenter : GenericPresenter<ICampaignsEditorView> {

        private IRepositoryFactory factory;

        public CampaignsEditorPresenter(IRepositoryFactory factory) {

            this.factory = factory;
        }

        public override void View_Load(object sender, EventArgs e)
        {
            var parameter = View.PostParameter;
            string action = parameter.Action;
            switch (action)
            { 
                case "SaveCampaign":
                    break;
                case "EditCampaign": 
                    EditCampaign();
                    break;
                
            }
        }

        private void EditCampaign()    {

            var parameter = View.PostParameter;
            Guid guid = parameter.DataTarget;
            var repository = factory.GetRepository<Repository<Campaigns>>();
            var query = repository.GetQueryable();

            var campaign = query.Where(o => o.Id == guid)
                                .Single();

            var screenData = campaign.Timelines.Select(o =>
                            new
                            {
                                TimelineId = o.Id,
                                Resolution = campaign.ScreenTemplate.Resolution.Resolution,
                                Orientation = campaign.ScreenTemplate.Resolution.Orientation.Orientation.ToString(),
                                ScreenType =   o.ScreenType.Id, 
                                ChannelBlocks = o.Channels.Select(channel => new
                                {
                                    cid = channel.Id,
                                    blocks = channel.Blocks.Select(block =>
                                        new
                                    {
                                        bid = block.Id,
                                        bdet = new
                                        {
                                            block.Length,
                                            block.Resource.ResourceType,
                                            block.Resource.Name
                                        }

                                    }).ToDictionary(prop => prop.bid, prop => prop.bdet)

                                }).ToDictionary(prop => prop.cid, prop => prop.blocks),
                                ViewerChannels = o.Channels.Select(channel => new
                                {
                                    sid = channel.ScreenDivision.Id,
                                    cid = channel.Id,

                                }).ToDictionary(prop => prop.sid, prop => prop.cid),
                                TotalDuration = o.TotalDuration,
                                BlockProperties = o.Channels
                                                   .SelectMany(block => block.Blocks)
                                                   .Select(c => new
                                                   {
                                                       Id = c.Id,
                                                       Properties = new 
                                                       {   
                                                            Type = c.Resource.ResourceType,
                                                            Length = c.Length,
                                                       }
                                                   })
                                                   .ToDictionary(prop => prop.Id, prop => prop.Properties)  
                            });

            var ScreenTemplate = new 
            {  
                Resolution = campaign.ScreenTemplate.Resolution.Resolution,
                Orientation = campaign.ScreenTemplate.Resolution.Orientation.Orientation.ToString()
            };

             


            View.JsonData = Json(
                new
                {
                   ScreenData = screenData,
                   CampaignId = guid,
                   ScreenTemplate = ScreenTemplate,
                   Timeline = View.PostParameter.Timeline,
                   Channel = View.PostParameter.Channel,
                }
            );
        }

        
    }
}