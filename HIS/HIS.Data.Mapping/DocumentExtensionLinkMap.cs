using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;

namespace HIS.Data.Mapping
{
    public class DocumentExtensionLinkMap : ClassMapping<DocumentExtensionLink>
    {
        public DocumentExtensionLinkMap()
        {

            Table("DocumentsLinks");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property(x => x.PathToPhysicalDocument, m => 
            {
                m.Access(NHibernate.Mapping.ByCode.Accessor.Field);
                m.Formula(@"(select file_stream.GetFileNamespacePath(1, 2) from Documents a where a.path_locator = DocumentPathLocator)");

            });

            Property(x => x.DocumentPathLocator, m => {
                m.Access(NHibernate.Mapping.ByCode.Accessor.Field);
            });

            ManyToOne<DocumentExtension>(x => x.DocumentExtension, m => 
            {
                m.Column("DocumentId");
                m.Cascade(Cascade.Persist);
                m.Access(Accessor.Field);
            
            });
        }
    }
}
