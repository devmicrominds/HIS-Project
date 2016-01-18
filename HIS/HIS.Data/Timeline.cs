using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Timeline : BaseDomain
    {
        public Timeline() {

            this.Channels = new HashSet<Channel>();
        }

        public virtual ScreenType ScreenType { get; set; }

        public virtual ICollection<Channel> Channels { get; set; }

        public virtual Campaigns Campaign { get; set; }

        public virtual int TotalDuration
        {
            get {

                return Channels.Max(o => o.Duration);
            }
            
        }



        public virtual void AddChannels(Channel channel) {

            this.Channels.Add(channel);
            channel.Timeline = this;
        }

        public virtual void AddChannels(IEnumerable<Channel> channels)
        {
             foreach(var channel in channels) {
                 this.AddChannels(channel);   
             }
        }

        public virtual void AddScreenType(ScreenType screenType) {
            this.ScreenType = screenType;
             
        }

        
    }
}
