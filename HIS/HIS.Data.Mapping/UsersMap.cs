using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class UsersMap : ClassMapping<Users>
    {
        public UsersMap() {

            Table("Users");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.UserName);

            Property<string>(x => x.UserPassword);

            Property<string>(x => x.Email);                  

            ManyToOne<Roles>(x => x.UserRoles, m => 
            {
                m.Cascade(Cascade.Persist);
            
            });

            
            
        }
    }
}
