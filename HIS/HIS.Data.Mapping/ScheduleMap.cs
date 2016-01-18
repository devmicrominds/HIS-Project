using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ScheduleMap : ClassMapping<Schedule>
    {
        public ScheduleMap() {

            Table("Schedule");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb)); 

            ManyToOne<Groups>(x => x.Groups, m => m.Cascade(Cascade.Persist));

            Bag<ScheduleEvent>(x => x.ScheduleEvents, m => {

                m.Key(km => km.Column(col => col.Name("ScheduleId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());
        }
    }
}
