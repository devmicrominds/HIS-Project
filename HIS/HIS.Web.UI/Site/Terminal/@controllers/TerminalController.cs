using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HIS.Web.UI
{
    public class TerminalController : ApiController
    {
        private IRepositoryFactory factory;

        public TerminalController(IRepositoryFactory factory)
        {

            this.factory = factory;
        }

        [HttpPost]
        public HttpResponseMessage GroupsScheduleSave(dynamic data)
        {
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);

                var crepository = factory.GetRepository<Repository<Campaigns>>();
                var grepository = factory.GetRepository<Repository<Groups>>();
                var repository = factory.GetRepository<Repository<Schedule>>();

                Guid groupId = data.group;
                var group = grepository.GetById(groupId, false);

                factory.OnTransaction(() =>
                {
                    

                    var schedule = new Schedule();
                    schedule.AttachGroup(group);

                    foreach (var @event in data.events)
                    {

                        Guid cid = @event.cid;
                        DateTime start = @event.start;
                        DateTime end = @event.end;
                        var campaign = crepository.GetById(cid, false);

                        var se = new ScheduleEvent()
                        {

                            Campaign = campaign,
                            DayOfWeek = start.DayOfWeek,
                            StartTime = start.GetTimeSpan(),
                            EndTime = end.GetTimeSpan(),

                        };

                        schedule.AddScheduleEvent(se);

                    }

                    repository.SaveOrUpdate(schedule);
                });

                response.Content = new JsonContent(new  { 
                      Group = groupId
                });

                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}