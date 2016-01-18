using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class RolesMap : ClassMapping<Roles>
    {
        public RolesMap() {

            Table("Roles");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.Name);

            Property<string>(x => x.Description);

            Set(x => x.UserRoles, m =>
            {
                m.Table("UsersRoles");

                m.Cascade(Cascade.All);

                m.Key(k => 
                {
                    k.Column("RolesId");                    
                });

            },
            map => map.ManyToMany(p => p.Column("UsersId"))); 

            Set(x => x.RolePrivileges, m =>
            {
                m.Table("RolesPrivileges");

                m.Cascade(Cascade.All);

                m.Key(k => { 
                    
                    k.Column("RolesId");
                    
                });

            },
            map => map.ManyToMany(p => p.Column("PrivilegesId")));
        }
    }
}
