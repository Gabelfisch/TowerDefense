using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Projekt
{
    internal class Checkpoint(int x, int y)
    {
        public int X { get; init; } = x;
        public int Y { get; init; } = y;
    }
}
