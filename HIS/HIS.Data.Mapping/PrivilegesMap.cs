using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
 

namespace HIS.Data.Mapping 
{
    public class PrivilegesMap : ClassMapping<Privileges>
    {
        public PrivilegesMap() {

            Table("Privileges");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.Code); 

            Property<string>(x => x.PrivilegeDesc);
        }
    }
}
