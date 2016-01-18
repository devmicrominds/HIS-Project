using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class GroupsMap : ClassMapping<Groups>
    {
        public GroupsMap()
        {
            Table("Groups");
            
            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.Name);

            Property<string>(x => x.Description);

            ManyToOne<Schedule>(x => x.Schedule, m =>
            {
                m.Cascade(Cascade.Persist);

            });

            Set<Player>(x => x.Players, m =>
            {

                m.Key(km => km.Column(col => col.Name("GroupsId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());

        }
    }

}
