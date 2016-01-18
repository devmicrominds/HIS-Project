using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Campaigns : BaseDomain
    {
        public Campaigns() {

            this.Timelines = new HashSet<Timeline>();
        }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual ScreenTemplate ScreenTemplate { get; set; }

        public virtual ICollection<Timeline> Timelines { get; set; }

        public virtual void AddTimeline(Timeline timeline)
        {
            this.Timelines.Add(timeline);
            timeline.Campaign = this;
        }

        public virtual int TotalDuration {

            get
            {

                return Timelines.Sum(o => o.TotalDuration);
            }
        }
    }
}
