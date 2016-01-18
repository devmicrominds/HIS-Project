using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class TimelineMap : ClassMapping<Timeline>
    {
        public TimelineMap() {

            Table("Timelines");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            ManyToOne<Campaigns>(x => x.Campaign, m =>
            {
                m.Column("CampaignId");

                m.Cascade(Cascade.Persist);

            });


            ManyToOne<ScreenType>(x => x.ScreenType, m => {

                m.Column("ScreenTypeId");

                m.Cascade(Cascade.Persist);
            
            });



            Set<Channel>(x => x.Channels, m =>
            {
                m.Key(km => km.Column(col => col.Name("TimelineId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());
        }
    }
}
