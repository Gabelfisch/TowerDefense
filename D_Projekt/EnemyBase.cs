﻿using System.Diagnostics;
using System.Threading.Tasks;

namespace D_Projekt
{
    internal abstract class EnemyBase
    {
        // For the cost of each enemy <subtype of EnemyBase, costs as int>
        public static Dictionary<Type, int> Costes = new() { {typeof(EnemyTank), 12} };

        private double[] directionToMove = new double[2]; //TODO: rename again

        public static Image Image { get; protected set; } = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "images", "Tank.png")); // Standard Image
        public static byte Speed { get; protected set; }
        public static int MaxHealth { get; protected set; }

        public int Costs { get; init; }
        public PointF LocationPointF { get; private set; }
        public RectangleF Bounds { get; private set; }
        public int Health { get; set; }
        public int IndexOfNextCheckpoint { get; set; } = 1;

        // For pathFinished event
        public delegate void PathFinishedHandler(EnemyBase sender, EventArgs e); 
        public event PathFinishedHandler PathFinishedEvent;

        // For death event
        public delegate void DeathHandler(EnemyBase sender, EventArgs eventArgs);
        public event DeathHandler DeathEvent;

        public EnemyBase(Checkpoint[] cpArray) //cpArray needed because there is a Error in the ctor of EnemyTank (CS1729)
        {
            //Debug.WriteLineIf(Costes.Keys.ToArray()[0] == typeof(EnemyTank), typeof(EnemyTank));
        }

        public virtual void SetPointAndRect(Checkpoint cp)
        {
            LocationPointF = new Point(cp.X, cp.Y);
            Bounds = new RectangleF(LocationPointF, new Size(50, 50));
        }

        public void CalculateDistanceToMove(Checkpoint[] cpArray)
        {

            Speed = Convert.ToByte(Math.Abs(Speed));
            
            double x = LocationPointF.X;
            double y = LocationPointF.Y;

            int cpX = cpArray[IndexOfNextCheckpoint].X;
            int cpY = cpArray[IndexOfNextCheckpoint].Y;

            double distanceX = Math.Abs(cpX - x);
            double distanceY = Math.Abs(cpY - y);
           
            double moveY;
            double moveX;

            //Debug.WriteLine(x +"|"+ y + "|"+"|" + cpX +"|"+ cpY);

            if (Math.Abs(distanceX) > Math.Abs(distanceY))
            {
                // the longer distance is set to the speed (or negative speed)
                if (cpX > x) moveX = Speed;
                else moveX = Speed * -1;
                

                if (cpY > y)
                { 
                    moveY = distanceY / (distanceX / Speed); 
                }
                else
                {
                    moveY = (distanceY / (distanceX / Speed)) * -1;
                }
            }
            else
            {
                if (cpY > y) moveY = Speed;
                else moveY = Speed * -1;

                if (cpX > x)
                {
                    moveX = distanceX / (distanceY / Speed);
                } 
                else
                {
                    moveX = (distanceX / (distanceY / Speed)) * -1;
                }
            }

            directionToMove = [moveX, moveY];
            
            //Debug.WriteLine(directionToMove[0]+ "|" + directionToMove[1]); 
        }

        public void MoveTowardsCheckpoint()
        {
            LocationPointF = new PointF((float)(LocationPointF.X + directionToMove[0]), (float)(LocationPointF.Y + directionToMove[1]));
            Bounds = new RectangleF(LocationPointF, new SizeF(Bounds.Width, Bounds.Height));  
        }

        public void IsCheckpointArrived(Checkpoint[] cpArray)
        {
            int cpX = cpArray[IndexOfNextCheckpoint].X;
            int cpY = cpArray[IndexOfNextCheckpoint].Y;
            if (Math.Abs(cpX - LocationPointF.X) <= Speed && Math.Abs(cpY - LocationPointF.Y) <= Speed)
            {
                Form1.moneyEnemys += Costs / Form1.allCheckpointsLvl1.Count;   
                IndexOfNextCheckpoint++;
                if (cpArray.Length == IndexOfNextCheckpoint)
                {
                    //TODO: Event machen bei letztem Checkpoint
                    OnPathFinished(EventArgs.Empty);
                    return;
                }
                CalculateDistanceToMove(cpArray);
            }
        }

        public void CheckDeath()
        {
            if (Health < 0) 
            {
                OnDeath();
            }
        }

        public void OnPathFinished(EventArgs e)
        {
            PathFinishedEvent?.Invoke(this, e);
        }

        public void OnDeath()
        {
            DeathEvent?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateEnemy(Checkpoint[] cpArray)
        {
            //CalculateDistanceToMove(cpArray);
            CheckDeath();
            MoveTowardsCheckpoint();
            IsCheckpointArrived(cpArray);
            
        }
    }
}