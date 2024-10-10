using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    internal class ShopManager
    {
        EnemyManager enemyManager;
        Player player;
        public int coinAmount;

        
        public void SetPlayer(Player player) 
        {
            this.player = player;
        }
        public void SetEnemyManager(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }


        public void Start() { }
        public void Update() 
        { 
        
        }
        public int ClaimBountyOn(string nameOfEnemy)
        {



            return coinAmount;
        }
    }
}
