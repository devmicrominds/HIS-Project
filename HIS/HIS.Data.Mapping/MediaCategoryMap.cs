using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class MediaCategoryMap : ClassMapping<MediaCategory>
    {
        public MediaCategoryMap()  {
   
            Table("MediaCategory");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));
            
            Property<ResourceType>(x => x.MediaResourceType);

            Property<string>(x => x.Name);

            Property<string>(x => x.ColorCode);

            Property<string>(x => x.Description);

            Set<Resource>(x => x.Resources, m =>
            {

                m.Key(km => km.Column(col => col.Name("MediaCategoryId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());
        }
    }
}
