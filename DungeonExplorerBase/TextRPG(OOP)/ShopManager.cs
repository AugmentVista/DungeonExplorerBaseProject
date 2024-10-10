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
        public int armourUpgradeCost;
        public int healthUpgradeCost;
        public int damageUpgradeCost;
        private int upgradeCostMultiplier = 4;

        public int armourUpgradesTaken = 0;
        public int healthUpgradesTaken = 0;
        public int damageUpgradesTaken = 0;


        public int coinAmount = 0;

        
        public void SetPlayer(Player player) 
        {
            this.player = player;
        }
        public void SetEnemyManager(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }

        public int ClaimBountyOn(string nameOfEnemy)
        {
            if (enemyManager.PlasmoidNames.Contains(nameOfEnemy))
            {
                for (int i = 0; i < nameOfEnemy.Length;)
                {
                    coinAmount++;
                }
            }
            else if (enemyManager.ConstructNames.Contains(nameOfEnemy))
            {
                for (int i = 0; i < nameOfEnemy.Length;)
                {
                    coinAmount++;
                }
            }
            else if (enemyManager.GoblinNames.Contains(nameOfEnemy)) 
            {
                for (int i = 0; i < nameOfEnemy.Length;)
                {
                    coinAmount++;
                }
            }

            return coinAmount;
        }


        public void UpdateUpgradeCosts(string upgradeType, int stock)
        {
            if (upgradeType == "Armor")
            {
                armourUpgradesTaken++;
                armourUpgradeCost = (stock - armourUpgradesTaken) * upgradeCostMultiplier;
            }
            if (upgradeType == "Health")
            { 
                healthUpgradesTaken++;
            }
            if (upgradeType == "Damage")
            { 
                damageUpgradesTaken++;
            }
        }

        public void Start()
        {

        }
        public void Update()
        {
        
        }
    }
}
