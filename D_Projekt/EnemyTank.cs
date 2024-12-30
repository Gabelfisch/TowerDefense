using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Projekt
{
    internal class EnemyTank : EnemyBase
    {

        public EnemyTank(Checkpoint[] cpArray) :base(cpArray)
        {   
            Speed = 6;
            MaxHealth = 150;
            Health = MaxHealth;
            Costs = 10;

            SetPointAndRect(cpArray[0]);
            CalculateDistanceToMove(cpArray);
        }



    }
}
