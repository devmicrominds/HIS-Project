using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Groups : BaseDomain
    {    

        public virtual string Description { get; set; }

        public virtual string Name { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual void AddSchedule(Schedule schedule)
        {
            this.Schedule = schedule;
            schedule.Groups = this;
        }
    }
}
