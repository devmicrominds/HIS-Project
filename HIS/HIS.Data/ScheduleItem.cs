using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ScheduleItem  : BaseDomain
    {
      

        public virtual DayOfWeek DayOfWeek { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual Schedule Schedule { get; set; }
    }
}
