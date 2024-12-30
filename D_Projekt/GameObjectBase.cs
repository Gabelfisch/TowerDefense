using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Projekt
{
    internal class GameObjectBase
    {
        // TODO: Ask Fr. Mayer if one image per instance but inherited is better, or one static image in each Class
        // if it stays like this, every Image of a Game object is the same!!!
        // TODO: Eigene Klasse mit lauter static bildern
        public RectangleF Bounds { get; protected set; }
        
    }
}
