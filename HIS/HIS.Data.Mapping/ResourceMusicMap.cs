using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ResourceMusicMap : JoinedSubclassMapping<ResourceMusic>
    {
        public ResourceMusicMap() {

            Table("ResourcesMusic");

            Key(m => m.Column("Id"));

            Property<string>(x => x.Artist);

            Property<string>(x => x.Album);

            Property<string>(x => x.Genre);

            Property<string>(x => x.Extension);

            Property<string>(x => x.Filename);

            Property<string>(x => x.Size);

            Property<string>(x => x.Length);
        }
    }
}
