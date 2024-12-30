using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Projekt
{
    //@MACO Its not possible to Inherit from Point
    internal sealed class Checkpoint(int x, int y)
    {
        public int X { get; init; } = x;
        public int Y { get; init; } = y;
    }
}
