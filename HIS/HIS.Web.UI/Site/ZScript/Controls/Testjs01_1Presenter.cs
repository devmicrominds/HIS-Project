using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.ZScript.Controls
{
    public class Testjs01_1Presenter : GenericPresenter<ITestjs01_1View>
    {
        private IRepositoryFactory factory;

        public Testjs01_1Presenter(IRepositoryFactory factory)
        {

            this.factory = factory;
             
        }

        public override void View_Load(object sender, EventArgs e)
        {
            EditCampaign();
        }

        private void EditCampaign()
        {
            Guid guid = new Guid("540482ae-202c-e411-9974-d34f6af8d096");
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
                                ScreenType = o.ScreenType.Id,
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

                }
            );
        }

    }
}