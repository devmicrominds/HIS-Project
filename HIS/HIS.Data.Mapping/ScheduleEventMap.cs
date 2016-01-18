using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ScheduleEventMap     : ClassMapping<ScheduleEvent>
    {
        public ScheduleEventMap() {

            Table("ScheduleEvent");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<DayOfWeek>(x => x.DayOfWeek);
                                         
            Property<string>(x => x.StartTime);

            Property<string>(x => x.EndTime);

            ManyToOne<Campaigns>(x => x.Campaign, m => {

                m.Column("CampaignsId");
                m.Cascade(Cascade.Persist);
            });

            ManyToOne<Schedule>(x => x.Schedule, m =>
            {
                m.Column("ScheduleId");
                m.Cascade(Cascade.Persist);

            });
        }
    }
}
