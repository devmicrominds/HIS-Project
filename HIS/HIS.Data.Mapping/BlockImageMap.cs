using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class BlockImageMap : JoinedSubclassMapping<BlockImage>
    {
        public BlockImageMap() {

            Table("BlocksImage");

            Key(m => m.Column("Id"));

            Property<string>(x => x.Stub);
        }
    }
}
