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


        public void UpdateUpgradeCosts(string upgradeType, int stock) // stock from 12 down to 10 becomes:
        {
            // as stock goes down price should go up
            if (upgradeType == "Armor")
            {
                armourUpgradesTaken++;
                armourUpgradeCost = (stock + armourUpgradesTaken * upgradeCostMultiplier);
                // first upgrade is free as X * 0 = 0 Second upgrade cost = stock which is now stock -2
                // third upgrade is equal to stock -4 * 2 and so on.
            }
            if (upgradeType == "Health")
            { 
                healthUpgradesTaken++;
                healthUpgradeCost = (stock + healthUpgradesTaken * upgradeCostMultiplier);
            }
            if (upgradeType == "Damage")
            { 
                damageUpgradesTaken++;
                damageUpgradeCost = (stock + damageUpgradesTaken * upgradeCostMultiplier);
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
