using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ChannelMap : ClassMapping<Channel>
    {
        public ChannelMap() {
            
            Table("Channels");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            ManyToOne<ScreenDivision>(x => x.ScreenDivision, m =>
            {
                m.Column("ScreenDivisionId");
                m.Cascade(Cascade.Persist);
            });

            ManyToOne<Timeline>(x => x.Timeline, m => 
            {
                m.Column("TimelineId");
                m.Cascade(Cascade.Persist);
            });

            Set<Block>(x => x.Blocks, m =>
            {

                m.Key(km => km.Column(col => col.Name("ChannelId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());
        }
    }
}
