using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Projekt
{
    internal static class Picture
    {
        public static Image Checkpoint { get; private set; } = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "images", "Checkpoint.png"));
        public static Image EnemyTank { get; private set; } = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "images", "Tank.png"));
        public static Image TowerBase { get; private set; } = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "images", "TowerDefault.png"));
        public static Image Projectile { get; private set; } = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "images", "ProjektileDefault.png"));
        public static Image BackGround { get; private set; } = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "images", "BG.png"));
        public static Image Heart { get; private set; } = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "images", "HeartTD.png"));
    }
}
