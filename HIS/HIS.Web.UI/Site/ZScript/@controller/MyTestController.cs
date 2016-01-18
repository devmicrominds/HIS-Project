using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace HIS.Web.UI.Site.ZScript._controller
{
    public class MyTestController : ApiController
    {
        /*http://localhost:56061/app/MyTest/GetData02*/

        private IRepositoryFactory factory;
        JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
        public MyTestController(IRepositoryFactory factory)
        {

            this.factory = factory;
        }


        [HttpGet]
        public dynamic GetData01()
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




            var result = Json(
                 new
                 {
                     ScreenData = screenData,
                     CampaignId = guid,
                     ScreenTemplate = ScreenTemplate,

                 }
             );
            // var TheJson = TheSerializer.Serialize(result);
            return result;
        }
        [HttpGet]
        public dynamic GetData02()
        {
            var screenTemplate = factory.GetRepository<Repository<ScreenOrientation>>();

            var sQuery = screenTemplate.GetQueryable();

            var list = sQuery.ToList();

            var result = list
                         .Select(o => new
                         {
                             or = o.Orientation,
                             s = o.ScreenResolutions
                                  .Select(w => new
                                  {
                                      resolution = w.Resolution,
                                      screenTypes = w.ScreenTypes
                                                     .Select(u => new
                                                     {
                                                         u.Id,
                                                         sd = u.ScreenDivisions
                                                               .Select(v =>
                                                                   new
                                                                   {
                                                                       sdn = v.Name,
                                                                       sdd = new
                                                                       {
                                                                           id = v.Id,
                                                                           x = v.X,
                                                                           y = v.Y,
                                                                           w = v.Width,
                                                                           h = v.Height
                                                                       }
                                                                   })
                                                               .ToDictionary(f => "sd" + f.sdn,
                                                                             f => f.sdd)

                                                     }).ToDictionary(z => z.Id,
                                                                     z => z.sd)

                                  }).ToDictionary(r => r.resolution,
                                                  r => r.screenTypes)

                         }).ToDictionary(t => t.or, t => t.s);





            return Json(result);
        }

        [HttpGet]
        public IEnumerable<Player> GetData03()
        {
            return new List<Player>()
            {
                new Player() { Name = "1" , Location = "Azrul"},
                new Player() { Name = "2", Location = "Firdaus" }
            };
        }
        /*public dynamic Get()
        {
            var repository = factory.GetRepository<Repository<Roles>>();
            var query = repository.GetQueryable();

            var result = new List<dynamic>() 
            { 
                new {
                    value = 0,
                    text = "",
                }
            
            };

            result.AddRange(query.Select(o =>
                                  new
                                  {
                                      value = o.Id,
                                      text = o.Name
                                  }));

            var TheJson = TheSerializer.Serialize(result);
            return TheJson;
        }*/
    }
}