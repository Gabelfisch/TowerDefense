using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Projekt
{
    internal class GameObjectEnemy_Tower : GameObjectBase
    {
        public PointF LocationPointF { get; protected set; }
        public int Costs { get; init; }
    }
}
