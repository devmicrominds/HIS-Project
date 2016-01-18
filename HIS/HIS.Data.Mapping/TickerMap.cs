using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace HIS.Data.Mapping
{    
    public class TickerMap : ClassMapping<Ticker>
    {
         public TickerMap()
         {

            Table("Ticker");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<Guid>(x => x.MId);

            Property<string>(x => x.Title);

            Property<bool>(x => x.Display);

            Property<int>(x => x.OrderBy);     
            
        }
    }
}
