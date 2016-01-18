using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ScheduleItemMap     : ClassMapping<ScheduleItem>
    {
        public ScheduleItemMap() {

            Table("ScheduleItem");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<DayOfWeek>(x => x.DayOfWeek);

            Property<DateTime>(x => x.StartDate);

            Property<DateTime>(x => x.EndDate);

            ManyToOne<Schedule>(x => x.Schedule, m =>
            {
                m.Cascade(Cascade.Persist);

            });
        }
    }
}
