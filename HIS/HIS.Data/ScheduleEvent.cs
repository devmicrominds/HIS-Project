using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ScheduleEvent  : BaseDomain    
    {
        public virtual DayOfWeek DayOfWeek { get; set; }

        public virtual string StartTime { get; set; }

        public virtual string EndTime { get; set; }

        public virtual Campaigns Campaign { get; set; }

        public virtual Schedule Schedule { get; set; }
    }
}
