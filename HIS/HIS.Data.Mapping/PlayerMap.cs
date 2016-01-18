using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class PlayerMap : ClassMapping<Player>
    {
        public PlayerMap () {

            Table("Players");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.Name);

            Property<string>(x => x.Location);

            Property<string>(x => x.IPAddress);

            ManyToOne<PlayerSettings>(x => x.PlayerSettings, m =>  
            {
                m.Column("PlayerSettingsId");

                m.Cascade(Cascade.Persist);

            });

            ManyToOne<Groups>(x => x.Groups, m =>  {

                m.Column("GroupsId");

                m.Cascade(Cascade.Persist);

            });
        }
    }
}
