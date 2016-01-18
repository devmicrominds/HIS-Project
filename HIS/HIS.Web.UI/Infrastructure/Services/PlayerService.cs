using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using HIS.Shared;


namespace HIS.Web.UI.Infrastructure
{
    public class PlayerServiceController : ApiController
    {

        private IRepositoryFactory factory;
        private ApplicationPath appPath;

        public PlayerServiceController(IRepositoryFactory factory,ApplicationPath appPath)
        {
            this.factory = factory;
            this.appPath = appPath;
        }


        /// <summary>
        /// data 
        /// {
        ///      IPAddr : 0.0.0.0
        /// }
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetGroupsSchedule(dynamic data)
        {

            // Todo
            // Match input with player
            // Get player group 
            // continue as below


            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);

                string IPAddress = data.IPAddress;

                Guid scheduleId = data.ScheduleId;

                var player = GetPlayerByIpAddress(IPAddress);

                var repository = factory.GetRepository<Repository<Groups>>();

                Guid groupID = player.Groups.Id;

                var group = repository.GetById(groupID, false);

                if (group.Schedule.Id == scheduleId)
                {
                    response.Content = new JsonContent(new DownloadQueryResult { IsCurrentSchedule = true });
                }
                else
                {
                    var schedule = group.Schedule;

                    var se = schedule.ScheduleEvents
                             .Select(w => w.Campaign)
                             .GroupBy(w => w.Id)
                             .Select(w => w.FirstOrDefault());

                    var scheduleContext = new ScheduleContext()
                    {
                        Id = schedule.Id
                    };
                    // campaign
                    scheduleContext.Campaigns = se
                            .Select(o => new
                            {
                                Id = o.Id,
                                Data = new CampaignData()
                                {

                                    Id = o.Id,
                                    TotalDuration = o.TotalDuration
                                }
                            }).ToDictionary(prop => prop.Id, prop => prop.Data);

                    // campaign timelines
                    scheduleContext.CampaignTimelines = se
                       .Select(o => new
                       {

                           Id = o.Id,
                           Timelines = o.Timelines
                                        .Select(w => new TimelineData()
                                        {
                                            Id = w.Id,
                                            Duration = w.TotalDuration,
                                            ScreenType = w.ScreenType.Name,
                                        })

                       }).ToDictionary(prop => prop.Id, prop => prop.Timelines.ToArray());

                    // timeline channels
                    scheduleContext.TimelineChannels = se
                        .SelectMany(o => o.Timelines)
                        .Select(o => new
                        {
                            Id = o.Id,
                            Channels = o.Channels.Select(w => new ChannelData()
                            {
                                Id = w.Id,
                                Name = w.ScreenDivision.Name,
                                TotalLength = w.Duration
                            })
                        })
                        .ToDictionary(prop => prop.Id, prop => prop.Channels.ToArray())
                        ;

                    var resources = se
                                   .SelectMany(o => o.Timelines.SelectMany(w => w.Channels))
                                   .SelectMany(o => o.Blocks)
                                   .Select(o => new ResourceData()
                                   {

                                       Id = o.Resource.Id,
                                       ResourceType = o.Resource.ResourceType.ToString(),
                                       Location = o.Resource.ResourcePath


                                   }).Distinct()
                                   .ToDictionary(o => o.Id, o => o)
                                   ;

                    // channel blocks
                    scheduleContext.ChannelBlocks = se
                        .SelectMany(o => o.Timelines.SelectMany(w => w.Channels))
                        .Select(o => new
                        {
                            Id = o.Id,
                            Blocks = o.Blocks.Select(w => new BlockData()
                                {
                                    Resources = resources[w.Resource.Id],
                                    Id = w.Id,
                                    Length = w.Length
                                })
                        })
                        .ToDictionary(prop => prop.Id, prop => prop.Blocks.ToArray());


                    // resources
                    scheduleContext.Resources = new List<ResourceData>(resources.Values);


                    scheduleContext.Schedules = schedule.ScheduleEvents
                                                        .Select(o => new
                                                        ScheduleData()
                                                        {
                                                            CampaignId = o.Campaign.Id,
                                                            DayOfWeek = o.DayOfWeek,
                                                            StartTime = o.StartTime,
                                                            ExpirationTime = o.EndTime

                                                        }).ToList();

                    response.Content = new JsonContent(new
                    DownloadQueryResult()
                    {
                        IsCurrentSchedule = false,
                        ScheduleContext = scheduleContext
                    });

                }


                return response;
            }
            catch (Exception ex)
            {
                // log this
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);

            }


        }

        public Player GetPlayerByIpAddress(string IPAddress)
        {

            var repository = factory.GetRepository<Repository<Player>>();
            var query = repository.GetQueryable();

            return query.Where(o => o.IPAddress == IPAddress)
                        .FirstOrDefault();

        }
    }
}