using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace TextRPG_OOP_
{
    internal class ShopManager
    {
        EnemyManager enemyManager;
        Player player;
        public static bool Paused;
        public ConsoleKeyInfo shopInput;

        public int playerCoins { get; set; } = 10;
        public int coinAmount { get; set; } = 0;
        private int costOfType { get; set; }
        public int deadguys { get; set; } = 0;

        public int armourUpgradeCost { get; set; } = 3;
        public int healthUpgradeCost { get; set; } = 1;
        public int damageUpgradeCost { get; set; } = 2;

        public int playerStatToIncrease { get; set; } = 0;


        public void SaveShop()
        {
            string filePath = "shopdata.json";
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, json);
        }

        public static ShopManager LoadShop()
        {
            string filePath = "shopdata.json";
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<ShopManager>(json);
        }



        public void OpenShop(string type)
        {
            Paused = true;
            switch (type)
            {
                case "Armour":
                    player.gameMap.UpdateArmorUIInfo(type);
                    costOfType = armourUpgradeCost;
                    UpdateUpgradeCosts(type);
                    break;
                case "Health":
                    player.gameMap.UpdateHealthUIInfo(type);
                    costOfType = healthUpgradeCost;
                    UpdateUpgradeCosts(type);
                    break;
                case "Damage":
                    player.gameMap.UpdateDamageUIInfo(type);
                    costOfType = damageUpgradeCost;
                    UpdateUpgradeCosts(type);
                    break;
            }
            Console.Clear();
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.WriteLine("░░░░░" + " You've Entered the Shop! " + "          ░░░░░");
            Console.WriteLine("░░░░░" + " This shop specializes in " + type + "    ░░░░░");
            Console.WriteLine("░░░░░" + " Press any key to view our deals " + "   ░░░░░");
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            Console.WriteLine("You have " + playerCoins + " Money to spend ");
            Console.WriteLine("Press V to purchase + 1 of " + type + " Cost: " + costOfType  + "       ░░░░░");
            Console.WriteLine("Press B to purchase + 2 of " + type + " Cost: " + costOfType * 2  + "       ░░░░░");
            Console.WriteLine("Press N to purchase + 3 of " + type + " Cost: " + costOfType * 3  + "       ░░░░░");
            Console.WriteLine("Press M to purchase + 5 of " + type + " Cost: " + costOfType * 5 + "       ░░░░░");
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
                    if (playerCoins < costOfType)
                    {
                        Console.WriteLine("Sorry, Link. I can't give credit. Come back when you're a little... mmmmm... richer!");
                        Console.ReadKey();
                        CloseShop();
                    }
                    else if (playerCoins >= costOfType)
                    {
                        playerStatToIncrease = 1;
                        playerCoins -= costOfType;
                        if (QuestManager.questsCompleted < 1) { QuestManager.questsCompleted = 1; }

                    }
                    break;
                case ConsoleKey.B:
                    if (playerCoins < costOfType * 2)
                    {
                        Console.WriteLine("Sorry, Link. I can't give credit. Come back when you're a little... mmmmm... richer!");
                        Console.ReadKey();
                        CloseShop();
                    }
                    else if (playerCoins >= costOfType * 2)
                    {
                        playerStatToIncrease = 2;
                        playerCoins -= costOfType * 2;
                        if (QuestManager.questsCompleted < 1) { QuestManager.questsCompleted = 1; }
                    }
                    break;
                case ConsoleKey.N:
                    if (playerCoins < costOfType * 3)
                    {
                        Console.WriteLine("Sorry, Link. I can't give credit. Come back when you're a little... mmmmm... richer!");
                        Console.ReadKey();
                        CloseShop();
                    }
                    else if (playerCoins >= costOfType * 3)
                    {
                        playerStatToIncrease = 3;
                        playerCoins -= costOfType * 3;
                        if (QuestManager.questsCompleted < 1) { QuestManager.questsCompleted = 1; }
                    }
                    break;
                case ConsoleKey.M:
                    if (playerCoins < costOfType * 5 )
                    {
                        Console.WriteLine("Sorry, Link. I can't give credit. Come back when you're a little... mmmmm... richer!");
                        Console.ReadKey();
                        CloseShop();
                    }
                    else if (playerCoins >= costOfType * 5)
                    {
                        playerStatToIncrease = 5;
                        playerCoins -= costOfType * 5;
                        if (QuestManager.questsCompleted < 1) { QuestManager.questsCompleted = 1; }
                    }
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
                    if (playerCoins >= costOfType) { player.playerArmourUps = player.playerArmourUps + playerStatToIncrease; }
                    
                    Console.WriteLine("You have purchased " + playerStatToIncrease + " " + type);
                    if (player.playerArmour >= Settings.playerMaxArmour)
                    { 
                    Console.WriteLine("Cannot exceed armour limit of: " + Settings.playerMaxArmour);
                    }
                    Console.ReadKey();
                    CloseShop();
                    break;
                case "Health":
                    if (playerCoins >= costOfType) { Settings.playerMaxHP += playerStatToIncrease; }
                    
                    Console.WriteLine("You have purchased " + playerStatToIncrease + " " + type);
                    Console.WriteLine("As as bonus, your health as been restored");
                    player.healthSystem.SetHealth(Settings.playerMaxHP);
                    Console.ReadKey();
                    CloseShop();
                    break;
                case "Damage":
                    if (playerCoins >= costOfType) { player.playerDamageUps = player.playerDamageUps + playerStatToIncrease; }
                        
                    Console.WriteLine("You have purchased " + playerStatToIncrease + " " + type);
                    if (player.playerDamage >= Settings.playerMaxDamage) 
                    { 
                        Console.WriteLine("Cannot exceed damage limit of: " + Settings.playerMaxDamage); 
                    }

                    Console.ReadKey();
                    CloseShop();
                    break;
            }
            Debug.WriteLine(playerStatToIncrease);
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
                coinAmount++;
                playerCoins += coinAmount;
                deadguys++;
                if (QuestManager.questsCompleted < 4 && deadguys > 10) { QuestManager.questsCompleted = 4; }
            }
            else if (enemyManager.ConstructNames.Contains(nameOfEnemy))
            {
                coinAmount += 2;
                playerCoins += coinAmount;
                deadguys++;
                if (QuestManager.questsCompleted < 4 && deadguys > 10) { QuestManager.questsCompleted = 4; }
            }
            else if (enemyManager.GoblinNames.Contains(nameOfEnemy)) 
            {
                coinAmount += 3;
                playerCoins += coinAmount;
                deadguys++;
                if (QuestManager.questsCompleted < 4 && deadguys > 10) { QuestManager.questsCompleted = 4; }
            }
            Debug.WriteLine(playerCoins);
        }

        public void UpdateUpgradeCosts(string upgradeType)
        {
            if (upgradeType == "Armor")
            {
                armourUpgradeCost++;
                Debug.WriteLine(armourUpgradeCost);
            }
            if (upgradeType == "Health")
            {
                healthUpgradeCost++;
                Debug.WriteLine(healthUpgradeCost);
            }
            if (upgradeType == "Damage")
            {
                damageUpgradeCost++;
                Debug.WriteLine(damageUpgradeCost);
            }
        }
    }
}
