using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    internal class ShopManager
    {
        EnemyManager enemyManager;
        Player player;

        public int playerCoins = 0;
        public int coinAmount = 0;
        private int upgradeCostMultiplier = 2;

        public int armourUpgradeCost;
        public int healthUpgradeCost;
        public int damageUpgradeCost;

        public int armourUpgradesTaken = 0;
        public int healthUpgradesTaken = 0;
        public int damageUpgradesTaken = 0;


        public void OpenShop() 
        {
            if (playerCoins < armourUpgradeCost || playerCoins < healthUpgradeCost || playerCoins < damageUpgradeCost)
            {
                player.shopping = false;
                return;
            }
            else if (playerCoins < armourUpgradeCost) { player.shopping = true; } // disables movement while shopping
        } 

        public void CloseShop() { player.shopping = false; }
        
        public void SetPlayer(Player player) 
        {
            this.player = player;
        }
        public void SetEnemyManager(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }

        public void ClaimBountyOn(string nameOfEnemy)
        {
            if (enemyManager.PlasmoidNames.Contains(nameOfEnemy))
            {
                coinAmount = 1;
                playerCoins += coinAmount;
            }
            else if (enemyManager.ConstructNames.Contains(nameOfEnemy))
            {
                coinAmount = 2;
                playerCoins += coinAmount;
            }
            else if (enemyManager.GoblinNames.Contains(nameOfEnemy)) 
            {
                coinAmount = 3;
                playerCoins += coinAmount;
            }
            Debug.WriteLine(playerCoins);
        }


        public void UpdateUpgradeCosts(string upgradeType)
        {
            // as stock goes down price should go up
            if (upgradeType == "Armor")
            {
                armourUpgradesTaken++;
                armourUpgradeCost = armourUpgradesTaken * upgradeCostMultiplier;
            }
            if (upgradeType == "Health")
            { 
                healthUpgradesTaken++;
                healthUpgradeCost = healthUpgradesTaken * upgradeCostMultiplier;
            }
            if (upgradeType == "Damage")
            {
                damageUpgradesTaken++;
                damageUpgradeCost = damageUpgradesTaken * upgradeCostMultiplier;
            }
        }

        public void Start()
        {

        }
        public void Update()
        {
            while (player.shopping)
            {
                Console.WriteLine("I broke the game loop");
            }
        }
    }
}
