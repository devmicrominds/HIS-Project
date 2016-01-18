using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class PlayerSettingsMap : ClassMapping<PlayerSettings> {

        public PlayerSettingsMap () {

            Table("PlayerSettings");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            ManyToOne<Player>(x => x.Player, m =>
            {
                m.Column("PlayersId");
                
                m.Cascade(Cascade.Persist);

            });
        }
    }
}
