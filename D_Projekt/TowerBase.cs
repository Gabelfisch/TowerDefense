using System.Diagnostics;

namespace D_Projekt
{
    class TowerBase : GameObjectEnemy_Tower
    {
        // For the cost of each tower <subtype of TowerBase, costs as int> 
        // TODO: Add Enum for every Type of tower for Readabillity
        public static Dictionary<Type, int> Costs = new() { { typeof(TowerBase), 30 } };

        private readonly Stopwatch cooldownStopwatch = new();

        public RectangleF Range { get; init; }

        public int RangeInt { get; init; }
        public int Damage { get; set; }
        public double Cooldown { get; init; }

        // For the Shoot event
        public delegate void ShootHandler(TowerBase sender, EventArgs e, EnemyBase target);
        public event ShootHandler ShootEvent;

        public TowerBase(int x, int y)
        {
            Bounds = new Rectangle(x, y, 50, 50);
            cooldownStopwatch.Start();

            //TODO: outsurce this when child class exists
            #region outsource
            RangeInt = 50;
            Range = new Rectangle(Convert.ToInt16(Bounds.X - RangeInt), Convert.ToInt16(Bounds.Y - RangeInt), Convert.ToInt16(RangeInt * 2 + Bounds.Width), Convert.ToInt16(RangeInt * 2 + Bounds.Height));
            Cooldown = 1.5;
            Damage = 74;
            base.Costs = 30;
            #endregion  
        }

        public void CheckIfEnemyInRange(EnemyBase[] enemyArray)
        {
            if (cooldownStopwatch.ElapsedMilliseconds > Cooldown * 1000)
            {
                foreach (EnemyBase enemy in enemyArray)
                {
                    if (Range.IntersectsWith(enemy.Bounds))
                    {
                        OnShoot(EventArgs.Empty, enemy); //TODO: find out how to set the event Args
                        cooldownStopwatch.Restart();
                        return; // makes that the tower only shoots one time.
                    }
                }
            }
        }

        public void OnShoot(EventArgs e, EnemyBase target)
        {
            if (ShootEvent != null)
            {
                ShootEvent(this, e, target); // Raise the event
            }
        }

        public void UptdateTower(EnemyBase[] enemyArray)
        {
            CheckIfEnemyInRange(enemyArray);
        }

    }
}
