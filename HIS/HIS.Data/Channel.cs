using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Channel : BaseDomain
    {
        public Channel() {

            this.Blocks = new HashSet<Block>();
        }
             
        public virtual ScreenDivision ScreenDivision { get; set; }

        public virtual Timeline Timeline { get; set; }

        public virtual ICollection<Block> Blocks { get; set; }

        public virtual int Duration
        {
            get {

                return Blocks.Sum(o => o.Length);
            }
        }

       


        public virtual Block AddBlocks(Block block)
        {
            if (null == this.Blocks)
                this.Blocks = new HashSet<Block>();

            block.Channel = this;

            this.Blocks.Add(block);

            return block;
        }
    }
}
