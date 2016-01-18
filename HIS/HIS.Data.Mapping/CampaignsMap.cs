using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class CampaignsMap : ClassMapping<Campaigns>
    {
        public CampaignsMap() {

            Table("Campaigns");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.Name);

            Property<string>(x => x.Description);

            ManyToOne<ScreenTemplate>(x => x.ScreenTemplate, m =>
            {
                m.Column("ScreenTemplateId");
                m.Cascade(Cascade.Persist);
                m.Lazy(LazyRelation.Proxy);

            });

            

            Set<Timeline>(x => x.Timelines, m =>
            {
                m.Key(km => km.Column(col => col.Name("CampaignId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());


        }
    }
}
