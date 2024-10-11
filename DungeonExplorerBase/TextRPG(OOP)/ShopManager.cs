using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    internal class ShopManager
    {
        EnemyManager enemyManager;
        Player player;
        public ConsoleKeyInfo shopInput;

        public int playerCoins = 0;
        public int coinAmount = 0;
        private int upgradeCostMultiplier = 2;

        public int armourUpgradeCost;
        public int healthUpgradeCost;
        public int damageUpgradeCost;

        public int armourUpgradesTaken = 0;
        public int healthUpgradesTaken = 0;
        public int damageUpgradesTaken = 0;

        public void OpenShop(string type)
        {
            Console.Clear();
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.WriteLine("░░░░░" + " You've Entered the Shop! " + "          ░░░░░"); // 25 char + 15 char
            Console.WriteLine("░░░░░" + " This shop specializes in " + type + "    ░░░░░");
            Console.WriteLine("░░░░░" + " Press any key to view our deals " + "   ░░░░░");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.WriteLine("Press V for level 1 upgrade +1 to " + type + " Cost: 5 money" + "          ░░░░░");
            Console.WriteLine("Press B for level 2 upgrade +2 to " + type + " Cost: 9 money" + "          ░░░░░");
            Console.WriteLine("Press N for level 3 upgrade +3 to " + type + " Cost: 13 money" + "         ░░░░░");
            Console.WriteLine("Press M for level 10 upgrade +10 to " + type + " Cost: 40 money" + "       ░░░░░");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("You may purchase the same upgrade more than once!");
            Console.WriteLine("░░░░░          ░░░░░");
            Console.WriteLine("░░░░░          ░░░░░");
            Console.WriteLine("Press Q to Exit the shop when you're finished ");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.ReadKey(true);
            PurchaseUpgrades(type);
        }

        public void PurchaseUpgrades(string Type)
        {
            shopInput = Console.ReadKey(true);
            if (shopInput.Key == ConsoleKey.V && playerCoins > damageUpgradeCost)
            {
                player.playerDamageUps += 1;
            }

            if (shopInput.Key == ConsoleKey.B && playerCoins > damageUpgradeCost)
            {
                player.playerDamageUps += 2;
            }

            if (shopInput.Key == ConsoleKey.N && playerCoins > damageUpgradeCost)
            {
                player.playerDamageUps += 3;
            }

            if (shopInput.Key == ConsoleKey.M && playerCoins > damageUpgradeCost)
            {
                player.playerDamageUps += 10;
            }

            else if (shopInput.Key == ConsoleKey.Q)
            {
                CloseShop();
            }
            else
            { 
                CloseShop();
            }
        }

        public void CloseShop() 
        { 
            player.shopping = false; 
            player.gameMap.Draw();
            player.gameMap.DrawEnemyLegend();
            player.gameMap.DrawItemLegend();
            player.Draw();
            player.itemManager.Draw();
            player.Update(); 

        }

        
        
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
        }
    }
}
