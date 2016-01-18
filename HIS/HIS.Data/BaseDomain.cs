using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class BaseDomain
    {
        public virtual Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            
            if (!this.GetType().Equals(obj.GetType()))
                return false;

            return this.Id.Equals(((BaseDomain)obj).Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
