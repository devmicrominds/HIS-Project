using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ResourceMap : ClassMapping<Resource>
    {
        public ResourceMap() {

            Table("Resources");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            ManyToOne<MediaCategory>(x =>
                x.MediaCategory, m => 
                { 
                    m.Cascade(Cascade.Persist);
                    m.Column("MediaCategoryId");
                }

            );

            Property<ResourceType>(x => x.ResourceType);

            Property<string>(x => x.Name);

            Property<string>(x => x.Description);
        }
    }
}
