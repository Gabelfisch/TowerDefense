using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D_Projekt
{
    class ProjectileBase : GameObjectBase
    {

        private readonly int speed;
        private float[] directionToMove = new float[2];

       
        public EnemyBase Target { get; protected set; }
        public int Damage { get; set; }

        // for target Hit event
        public delegate void TargetHitHandler(ProjectileBase sender, EnemyBase target, EventArgs e);
        public event TargetHitHandler TargetHit;

        public ProjectileBase(PointF point, EnemyBase target, TowerBase sender) 
            
        {
            // outsource into child class
            speed = 15;
            Damage = sender.Damage;
            Bounds = new RectangleF(point, new SizeF(10, 10));
            this.Target = target;
        }
        
        public void CalculateDirection()
        {
            float x = Bounds.X;
            float y = Bounds.Y;

            float targetX = Target.Bounds.X;
            float targetY = Target.Bounds.Y;

            float distanceX = Math.Abs(targetX - x);
            float distanceY = Math.Abs(targetY - y);

            float moveY;
            float moveX;


            if (Math.Abs(distanceX) > Math.Abs(distanceY))
            {
                // the longer distance is set to the speed (or negative speed)
                if (targetX > x) moveX = speed;
                else moveX = speed * -1;


                if (targetY > y)
                {
                    moveY = distanceY / (distanceX / speed);
                }
                else
                {
                    moveY = (distanceY / (distanceX / speed)) * -1;
                }
            }
            else
            {
                if (targetY > y) moveY = speed;
                else moveY = speed * -1;

                if (targetX > x)
                {
                    moveX = distanceX / (distanceY / speed);
                }
                else
                {
                    moveX = (distanceX / (distanceY / speed)) * -1;
                }
            }

            directionToMove = [moveX, moveY];
            //Debug.WriteLine(moveX + "|" + moveY);
        }

        private void MoveProjectile()
        {
            Bounds = new RectangleF(Bounds.Left + directionToMove[0] , Bounds.Top + directionToMove[1], Bounds.Width, Bounds.Height);
        }

        private void CheckIfTargetHit()
        {
            if (Target.Bounds.IntersectsWith(Bounds))
            {
                OnTargetHit();
            }
        }

        private void OnTargetHit()
        {
            if (TargetHit != null)
            {
                TargetHit(this, Target, EventArgs.Empty);
            }
        }

        public void UpdateProjectile()
        {
            CheckIfTargetHit();
            CalculateDirection();
            MoveProjectile();
        }


    }
}
