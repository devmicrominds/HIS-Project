using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ScreenTemplateMap  : ClassMapping<ScreenTemplate>
    {
        public ScreenTemplateMap() {

            Table("ScreenTemplate");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            ManyToOne<ScreenResolution>(x => x.Resolution, m =>
            {
                m.Column("ScreenResolutionId");

                m.Cascade(Cascade.Persist);
            });

           
        }
    }
}
