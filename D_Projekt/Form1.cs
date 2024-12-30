using System.ComponentModel;
using System.Diagnostics;

namespace D_Projekt
{
    public partial class Form1 : Form
    {
        //TODO: make more enemys and towers
        //TODO: balancing
        //TODO: separate keyboard input for enemys and mouse for tanks
        //TODO: make a new image for projectiles, also a background
        //TODO: if (motivation and time) make different game modes or stages

        // ------------------ fields ------------------
        #region fields

        // For the UI
        private readonly Bitmap bitmap; //TODO: Ask Fr. Mayer why static and readonly are allowed on their own, but not together

        private static readonly List<Image> allLives = [];

        // Lists of the game Objects
        internal static readonly List<Checkpoint> allCheckpointsLvl1 = [new Checkpoint(-50, 215), // internal because must be readable in enemy base, and cant be public bc of CS0052 
                                                                new Checkpoint(430, 663),
                                                                new Checkpoint(861, 708),
                                                                new Checkpoint(800, 130),
                                                                new Checkpoint(232,82),
                                                                new Checkpoint(130, 681)];
        private static readonly List<EnemyBase> allEnemies = [];
        private static readonly List<TowerBase> allTowers = [];
        private static readonly List<ProjectileBase> allProjectiles = [];

        // Timer for the GameTick/GameLoop
        private static System.Windows.Forms.Timer gameTick = new()
        {
            Interval = 20,
            Enabled = true
        };
        // Timer for giving out money over time
        private static System.Windows.Forms.Timer moneyTick = new()
        {
            Interval = 5000,
            Enabled = true
        };


        // for placing the tower
        private static bool isTowerPlacing = false;

        // money system
        private static int moneyTowers = 60;
        public static int moneyEnemys = 30; // public because must be editable in enemy base
        private static int everyTickMoneyEnemys = 12;
        private static int everyTickMoneyTowers = 5;

        // for stats at the end
        private static int projectilesShot = 0;
        private static int enemysSpawned = 0;
        private static int enemysKilled = 0;
        private static int towersPlaced = 0;

        #endregion
        // ------------------ ctor ------------------
        #region ctor

        public Form1()
        {
            InitializeComponent();

            // Timers for Adding Money over Time
            gameTick.Tick += GameTick_Tick; 
            moneyTick.Tick += MoneyTick_Tick;

            // Bitmap where everthing is drawn on.
            bitmap = new Bitmap(ClientSize.Width, ClientSize.Height);

            DoubleBuffered = true; // To avoid Flickering IMPORTANT haha
            KeyPreview = true; // This is needed for handling keydown events, even if eg. a button in in focus

            // fill up lives
            AddLives();
        }

        #endregion
        // ------------------ functions ------------------
        #region functions

        /// <summary>
        /// Adds lives in the beginning of the game
        /// </summary>
        private void AddLives()
        {
            for (int i = 0; i < 10; i++)
            {
                allLives.Add(Picture.Heart);
            }
        }

        /// <summary>
        /// Draws all the objects onto a bitmap, that will be painted later in the Form1_Paint Paint event which
        /// is executed when the Frame is Invalidated (happens in GameTick_Tick)
        /// </summary>
        private void DrawObjects()
        {
            // creating Graphics Object where erverything is Drawn on, based on a Bitmap. 
            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            // Draw background
            graphics.DrawImage(Picture.BackGround, 0, 0, 1000, 800);

            // Draw Projectiles (later before towers, after enemies
            allProjectiles.ForEach(proj => graphics.DrawImage(Picture.Projectile, proj.Bounds.Location));

            // Draw Towers
            allTowers.ForEach(tow => graphics.DrawImage(Picture.TowerBase, tow.Bounds));

            // Draw enemys
            allEnemies.ForEach(enemy => graphics.DrawImage(Picture.EnemyTank, enemy.LocationPointF.X, enemy.LocationPointF.Y, 30, 30));
            
            // Draw hearts
            int XOfHeart = 20;
            foreach (Image live in allLives)
            {
                graphics.DrawImage(live, new RectangleF(new PointF(XOfHeart, 20), new Size(30, 30)));
                XOfHeart += 40;
            }

            //Draw Tower Range
            allTowers.ForEach(tow => graphics.DrawRectangle(new Pen(Color.Blue), tow.Range));
 
            // ------- Debug visualisations -------
            #region Debug

            // Debug where Checkpoints are, uncomment for visible Checkpoints
            //foreach (Checkpoint cp in allCheckpointsLvl1)
            //{
            //    graphics.DrawImage(Pictures.Checkpoint, cp.X, cp.Y, 50, 50);
            //}

            #endregion 

        }

        /// <summary>
        /// Places instance of TowerBase at Position int x and int Y, 
        /// adds the Shoot event to the new instance
        /// </summary>
        /// <param name="x">left top corner X coordinate position, integer</param>
        /// <param name="y">left top corner Y coordinate position, integer</param>
        public void PlaceNewTower(int x, int y)
        {
            if (moneyTowers >= TowerBase.Costs[typeof(TowerBase)]) //Modify Class for diff. towers
            {
                isTowerPlacing = false;
                moneyTowers -= TowerBase.Costs[typeof(TowerBase)];
                towersPlaced++;

                allTowers.Add(new TowerBase(x, y));
                allTowers[^1].ShootEvent += TowerBase_Shoot;
            }
        }

        /// <summary>
        /// Spawns a new Enemy at the first checkpoint
        /// </summary>
        public void SpawnEnemy()
        {
            if (moneyEnemys >= 12)
            {
                allEnemies.Add(new EnemyTank([.. allCheckpointsLvl1]));
                allEnemies[^1].PathFinishedEvent += EnemyBase_PathFinished;
                allEnemies[^1].DeathEvent += EnemyBase_Death;
                enemysSpawned++;

                moneyEnemys -= EnemyBase.Costes[allEnemies[^1].GetType()];
            }
        }


        /// <summary>
        /// Will be executed when the tower player is dead (all lives are gone)
        /// Shows some game statistics and asks for a new game
        /// </summary>
        public void GameFinished()
        {
            // Stop timer to stop the game 
            gameTick.Stop();

            // Send out Death message and statistics
            string deathMessage = $"Game Finished!!!\n" +
                $"Statistics:\n" +
                $"Towers Placed:    {towersPlaced}\n" +
                $"Enemies Swpawned:   {enemysSpawned}\n" +
                $"Enemies Killed:   {enemysKilled}\n" +
                $"Projectiles shot: {projectilesShot}\n" +
                $"To continue, click OK";

            MessageBox.Show(deathMessage, "Game Finished", MessageBoxButtons.OK);

            // Ask Player if he wants to play another game
            bool isAgain = MessageBox.Show("Do you want to play another round?", "Another one?", MessageBoxButtons.YesNo) == DialogResult.Yes ? true : false;
            
            if (isAgain)
                ResetandRestartGame();
            else
                Close();
            
        }

        /// <summary>
        /// restarts the game (kills enemys, towers and projectiles)
        /// refills lives and starts the game tick
        /// </summary>
        public void ResetandRestartGame()
        {
            
            // Delete all Enemies
            foreach (EnemyBase enemy in allEnemies)
            {
                enemy.DeathEvent -= EnemyBase_Death;
                enemy.PathFinishedEvent -= EnemyBase_PathFinished;
            }
            allEnemies.Clear();

            // Delete all Projectiles
            foreach (ProjectileBase proj in allProjectiles)
                proj.TargetHit -= ProjectileBase_TargetHit;
            allProjectiles.Clear();

            // Delete all Towers
            foreach (TowerBase tower in allTowers)
                tower.ShootEvent -= TowerBase_Shoot;
            allTowers.Clear();

            // refill lives
            AddLives();

            // reset money
            moneyTowers = 60;
            moneyEnemys = 30;

            // start the game
            gameTick.Start();
        }

#endregion
        // ------------------ event handler ------------------
        #region event handler

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(bitmap, 0, 0);
        }

        private void btn_spawnTower_MouseClick(object sender, MouseEventArgs e)
        {
            isTowerPlacing = !isTowerPlacing;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (isTowerPlacing) PlaceNewTower(e.Location.X, e.Location.Y);
                    break;
                case MouseButtons.Middle:
                    Debug.WriteLine($"{e.Location.X} | {e.Location.Y}"); 
                    break;
                case MouseButtons.XButton1:
                    isTowerPlacing = !isTowerPlacing;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("ads");
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Debug.WriteLine("ssdasdf");
                    Close(); 
                    break;
                case Keys.T:
                    SpawnEnemy();
                    break;
            }
        }

        private void TowerBase_Shoot(TowerBase sender, EventArgs e, EnemyBase target)
        {
            // Create new Projectile
            allProjectiles.Add(new ProjectileBase(sender.Bounds.Location, target, sender));
            allProjectiles[^1].TargetHit += ProjectileBase_TargetHit;
            
            // Uptdate Statistics
            projectilesShot++;
        }

        private void EnemyBase_PathFinished(EnemyBase sender, EventArgs e)
        {
            // make the Enemy ready to get Garbage collected
            sender.PathFinishedEvent -= EnemyBase_PathFinished;
            sender.DeathEvent -= EnemyBase_PathFinished;
            allEnemies.Remove(sender);


            // Remove a Live
            try
            {
                allLives.RemoveAt(0);
                moneyEnemys += 30;
            }
            catch
            {
                GameFinished();
            }
        }

        private void ProjectileBase_TargetHit(ProjectileBase sender, EnemyBase target, EventArgs e)
        {
            sender.TargetHit -= ProjectileBase_TargetHit;
            allProjectiles.Remove(sender);
            target.Health -= sender.Damage;
            Debug.WriteLine(target.Health);
        }

        private void EnemyBase_Death(EnemyBase sender, EventArgs e)
        {
            ProjectileBase[] allProjectilesCopy = allProjectiles.ToArray();

            foreach (ProjectileBase proj in allProjectilesCopy)
            {
                if (proj.Target == sender)
                {
                    proj.TargetHit -= ProjectileBase_TargetHit;
                    allProjectiles.Remove(proj);
                }
            }

            sender.PathFinishedEvent -= EnemyBase_PathFinished;
            sender.DeathEvent -= EnemyBase_Death;
            allEnemies.Remove(sender);
            moneyTowers += Convert.ToInt16(sender.Costs * 1.2);
            enemysKilled++;
        } 


        #endregion
        // ------------------ game loop and other timers ------------------
        #region game loop
        private void GameTick_Tick(object? sender, EventArgs e)
        {
            // Copie the lists, because else the lists can't be modified
            EnemyBase[] allEnemiesCopy = [.. allEnemies];
            TowerBase[] allTowersCopy = [.. allTowers];
            ProjectileBase[] allProjectilesCopy = [.. allProjectiles];

            // Updates all the game objects
            Array.ForEach(allEnemiesCopy, e => e.UpdateEnemy([.. allCheckpointsLvl1]));
            Array.ForEach(allTowersCopy, tw => tw.UptdateTower([.. allEnemies]));
            Array.ForEach(allProjectilesCopy, tw => tw.UpdateProjectile());

            // Display Money
            lbl_moneyEnemys.Text = $"E: {moneyEnemys}";
            lbl_moneyTowers.Text = $"T: {moneyTowers}";


            // Draws the objects on the Screen
            DrawObjects();
            this.Invalidate();

        }

        private void MoneyTick_Tick(Object? sender, EventArgs e)
        {
            // adding money over time
            moneyEnemys += everyTickMoneyEnemys;
            moneyTowers += everyTickMoneyTowers;
        }
        #endregion


    }
}
