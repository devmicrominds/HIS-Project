using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class DocumentExtensionMap : ClassMapping<DocumentExtension>
    {
        public DocumentExtensionMap() {

            Table("Documents");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Set<DocumentExtensionLink>(x => x.DocumentLinks, m =>
            {
                m.Access(Accessor.Field);

                m.Key(km => km.Column(col => col.Name("DocumentId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());
        }
    }
}
