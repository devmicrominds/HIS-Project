using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Schedule : BaseDomain
    {
        public Schedule() {

            this.ScheduleEvents = new List<ScheduleEvent>();
        }

        public virtual Groups Groups { get; set; }

        public virtual ICollection<ScheduleEvent> ScheduleEvents { get; set; }



        public virtual void AttachGroup(Groups group)
        {
            this.Groups = group;
            group.Schedule = this;
        }

        public virtual void AddScheduleEvent(ScheduleEvent se)
        {
            se.Schedule = this;
            this.ScheduleEvents.Add(se);
        }
    }
}
