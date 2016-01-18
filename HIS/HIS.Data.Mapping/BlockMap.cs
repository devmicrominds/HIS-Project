using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class BlockMap : ClassMapping<Block>
    {
        public BlockMap() {

            Table("Blocks");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<int>(x => x.Length);   
            

            ManyToOne<Resource>(x => x.Resource, m => {

                m.Column("ResourceId");

                m.Cascade(Cascade.Persist);
            });

            ManyToOne<Channel>(x => x.Channel, m => {
    
                m.Column("ChannelId");

                m.Cascade(Cascade.Persist);
            });
        }
    }
}
