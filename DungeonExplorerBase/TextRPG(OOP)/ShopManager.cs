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
        public static bool Paused;
        public ConsoleKeyInfo shopInput;

        public int playerCoins = 5;
        public int coinAmount = 0;
        private int upgradeCostMultiplier = 2;
        private int costOfType;

        public int armourUpgradeCost;
        public int healthUpgradeCost;
        public int damageUpgradeCost;

        public int armourUpgradesTaken = 0;
        public int healthUpgradesTaken = 0;
        public int damageUpgradesTaken = 0;

        public int playerStatToIncrease = 0;

        public void OpenShop(string type)
        {
            Paused = true;
            switch (type)
            {
                case "Armour":
                    UpdateUpgradeCosts("Armour", 1);
                    costOfType = armourUpgradeCost;
                    Console.WriteLine();
                    break;
                case "Health":
                    UpdateUpgradeCosts("Health", 1);
                    costOfType = healthUpgradeCost;
                    Console.WriteLine();
                    break;
                case "Damage":
                    UpdateUpgradeCosts("Damage", 1);
                    costOfType = damageUpgradesTaken;
                    Console.WriteLine();
                    break;
            }

            Console.Clear();
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.WriteLine("░░░░░" + " You've Entered the Shop! " + "          ░░░░░"); // 25 char + 15 char
            Console.WriteLine("░░░░░" + " This shop specializes in " + type + "    ░░░░░");
            Console.WriteLine("░░░░░" + " Press any key to view our deals " + "   ░░░░░");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.WriteLine("You have " + playerCoins + " Money to spend ");
            Console.WriteLine("Press V to purchase + 1 of " + type + " Cost: " + costOfType * 1 + "       ░░░░░");
            Console.WriteLine("Press B to purchase + 2 of " + type + " Cost: " + costOfType * 2 + "       ░░░░░");
            Console.WriteLine("Press N to purchase + 3 of " + type + " Cost: " + costOfType * 3 + "       ░░░░░");
            Console.WriteLine("Press M to purchase + 5 of " + type + " Cost: " + costOfType * 5 + "       ░░░░░");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press Q to Exit the shop when you're finished ");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            PurchaseUpgrades(type);
        }

        public void PurchaseUpgrades(string type)
        {
            shopInput = Console.ReadKey(true);


            switch (shopInput.Key)
            {
                case ConsoleKey.V:
                    playerStatToIncrease = 1;
                    break;
                case ConsoleKey.B:
                    playerStatToIncrease = 2;
                    break;
                case ConsoleKey.N:
                    playerStatToIncrease = 3;
                    break;
                case ConsoleKey.M:
                    playerStatToIncrease = 5;
                    break;
                case ConsoleKey.Q:
                    playerStatToIncrease = 0;
                    Console.Clear();
                    CloseShop();
                    break;
                default:
                    playerStatToIncrease = 0;
                    Console.Clear();
                    CloseShop();
                    break;
            }

            switch (type)
            {
                case "Armour":
                    player.playerArmourUps += playerStatToIncrease;
                    Console.WriteLine("You have purchased " + playerStatToIncrease + " " + type);
                    Console.WriteLine("Cannot exceed armour limit of: " + Settings.playerMaxArmour);
                    Console.ReadKey();
                    CloseShop();
                    break;
                case "Health":
                    player.playerHealthUps += playerStatToIncrease;
                    Console.WriteLine("You have purchased " + playerStatToIncrease + " " + type);
                    Console.ReadKey();
                    CloseShop();
                    break;
                case "Damage":
                    player.playerDamageUps += playerStatToIncrease;
                    Console.WriteLine("You have purchased " + playerStatToIncrease + " " + type);
                    if (player.playerDamage >= Settings.playerMaxDamage) 
                    { 
                        Console.WriteLine("Cannot exceed damage limit of: " + Settings.playerMaxDamage); 
                    }
                    Console.ReadKey();
                    CloseShop();
                    break;
            }
        }

        public void CloseShop() 
        {
            Console.Clear();
            player.shopping = false;
            Paused = false;
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

        public void UpdateUpgradeCosts(string upgradeType, int amountPurchased)
        {
            // as stock goes down price should go up
            if (upgradeType == "Armor")
            {
                armourUpgradesTaken += amountPurchased;
                armourUpgradeCost = armourUpgradesTaken * upgradeCostMultiplier;
            }
            if (upgradeType == "Health")
            { 
                healthUpgradesTaken += amountPurchased;
                healthUpgradeCost = healthUpgradesTaken * upgradeCostMultiplier;
            }
            if (upgradeType == "Damage")
            {
                damageUpgradesTaken += amountPurchased;
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
