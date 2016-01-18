using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS.Web.UI.Site
{
    public class GroupsSchedulePresenter : GenericPresenter<IGroupsScheduleView>  
    {

        private IRepositoryFactory factory;
        
        public GroupsSchedulePresenter(IRepositoryFactory factory) {
            
            this.factory = factory;
        }

        public override void View_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()   {

            var group = Groups();
            var events = group.Schedule.ScheduleEvents.Select(o =>
                new 
                { 
                    cid=o.Campaign.Id,
                    title= o.Campaign.Name, 
                    start = GetScheduleDate(o.DayOfWeek,o.StartTime), 
                    end = GetScheduleDate(o.DayOfWeek,o.EndTime),
                    allDay = false
                      
                }).ToArray();

            string filter = View.PostParameter.Filter;   

            var repository = factory.GetRepository<Repository<Campaigns>>();

            var query = repository.GetQueryable();

            if (!String.IsNullOrEmpty(filter)) 
                query = query.Where(o => o.Name.Contains(filter));


            var campaigns = query.Select(o => new { cid = o.Id, name = o.Name })
                            .Take(5)
                            .ToList();

            View.JsonData = Json(new {

                Group = group.Id,
                Campaigns = campaigns,
                Events = events
            });
        
        }

        private DateTime GetScheduleDate(DayOfWeek dayofweek,string timespan) 
        {
            var date = DateTime.Now.StartOfWeek(dayofweek);
            return date.FromTimeSpan(timespan);
        }

        private Groups Groups() {

            var parameter = View.PostParameter;
            Guid uid = parameter.DataTarget;

            var repository = factory.GetRepository<Repository<Groups>>();
             

            return repository.GetById(uid, false);



        }
    }
}
