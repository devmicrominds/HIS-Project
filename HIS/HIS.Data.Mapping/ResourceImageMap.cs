using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HIS.Data.Mapping
{
    public class ResourceImageMap : JoinedSubclassMapping<ResourceImage>
    {
        public ResourceImageMap()
        {
            Table("ResourcesImage");

            Key(m => m.Column("Id"));

            Property<string>(x => x.Location);

            Property<string>(x => x.Filename);

            Property<string>(x => x.Extension);

            Property<string>(x => x.Size);

            Property<string>(x => x.Width);

            Property<string>(x => x.Height);

            Property<string>(x => x.Dimension);

            Property<byte[]>(x => x.Thumbnail,m=>m.Length(int.MaxValue));

        }
    }
}
