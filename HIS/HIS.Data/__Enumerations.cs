using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public enum Orientation 
    { 
        HORIZONTAL = 1,
        VERTICAL = 2,
    }

    public enum ResourceType { 

        Music = 1,
        Image = 2,
        Video = 3,
        Stream = 4, 
        HTML = 5,
        Ticker =6,
        CCTV=7,
    }

    

    public enum ImageProperties 
    { 
        Name = 0,
        Size = 1,
        Dimensions = 31,
        Width = 162,
        Height = 164,
    
    }

    public enum MusicProperties {
    
        Name = 0,
        Size = 1,
        ContributingArtist = 13,
        Album=14,
        Genre=16,
        Length = 27,
        
    
    }

    public enum VideoProperties {

        Name = 0,
        Size = 1,
        Title=21,
        Dimensions = 31,
        Length = 27,
        Width = 162,
        Height = 164,
        

    }
}
