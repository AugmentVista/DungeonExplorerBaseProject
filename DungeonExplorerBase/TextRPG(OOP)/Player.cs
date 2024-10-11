using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Is the player, Handles all player movment and interactions.
    /// </summary>
    internal class Player : Character
    {
        public ItemManager itemManager;
        public ShopManager shop;
        public QuestManager quest;
        public Map gameMap;

        public ConsoleKeyInfo playerInput;
        public int playerDamage;
        public int playerArmour;
        public int playerHealth;

        public int playerArmourUps = 1;
        public int playerHealthUps;
        public int playerDamageUps = 1;
        public int startingHealth = 20;

        public int enemyHitHealth;
        public int enemyHitArmor;

        public bool gameIsOver;
        public bool gameWon;
        public bool shopping;

        public string enemyHitName;

        public char avatar;

        public Player(Map map, ItemManager IM, ShopManager shop, QuestManager quest)
        {
            avatar = ((char)2); // Sets player to smiley face.
            healthSystem.IsAlive = true; // initilizes player as alive.
            gameIsOver = false;
            gameWon = false;

            healthSystem.health = startingHealth;
            playerDamage = playerDamageUps;
            playerHealth = healthSystem.GetHealth();
            playerArmour = playerArmourUps;

            name = "Player";
            enemyHitName = ""; // clears enemy hit for starting
            gameMap = map; // hands map to player
            itemManager = IM; // hands item manager to player
            this.shop = shop;
            shop.SetPlayer(this);
            this.quest = quest;
            quest.SetPlayer(this);
        }


        public void Start()
        {
            SetMaxPlayerPosition(gameMap);
        }


        public void Update()
        {
            playerHealth = healthSystem.GetHealth();
            playerArmour = playerArmourUps;
            playerDamage = playerDamageUps;
            CapPlayerStats();
            GetPlayerInput(gameMap);
            if (shop.playerCoins >= 20 && (QuestManager.questsCompleted < 2))
            { 
               QuestManager.questsCompleted = 2;  
            }
        }


        private void CapPlayerStats()
        {
            if (playerHealth > Settings.playerMaxHP) { playerHealth = Settings.playerMaxHP; }
            if (playerArmour > Settings.playerMaxArmour) { playerArmour = Settings.playerMaxArmour; }
            if (playerDamage > Settings.playerMaxDamage) { playerDamage = Settings.playerMaxDamage; }
        }
        public void SetMaxPlayerPosition(Map map)
        {
            int mapX;
            int mapY;
            mapX = map.activeMap.GetLength(1);
            mapY = map.activeMap.GetLength(0);
            position.maxX = mapX - 1;
            position.maxY = mapY - 1;
        }


        public void SetPlayerPosition(int x, int y)
        {
            position.x = x;
            position.y = y;
        }


        public void ItemCheck(Map collisionMap)
        {
            if (itemManager.items[collisionMap.itemIndex].itemType == "Health Pickup" && healthSystem.health < Settings.playerMaxHP)
            {
                TriggerShop("Health");
                if (itemManager.items[collisionMap.itemIndex].isActive != false)
                {
                    itemManager.items[collisionMap.itemIndex].isActive = false;
                    itemManager.items[collisionMap.itemIndex].position.x = 0;
                    itemManager.items[collisionMap.itemIndex].position.y = 0;
                }
            }
            if (itemManager.items[collisionMap.itemIndex].itemType == "Armor Pickup" && playerArmour < Settings.playerMaxArmour)
            {
                TriggerShop("Armour");
                if (itemManager.items[collisionMap.itemIndex].isActive != false)
                {
                    itemManager.items[collisionMap.itemIndex].isActive = false;
                    itemManager.items[collisionMap.itemIndex].position.x = 0;
                    itemManager.items[collisionMap.itemIndex].position.y = 0;
                }
            }
            if (itemManager.items[collisionMap.itemIndex].itemType == "Damage Pickup" && playerDamage < Settings.playerMaxDamage)
            {
                TriggerShop("Damage");
                if (itemManager.items[collisionMap.itemIndex].isActive != false)
                {
                    itemManager.items[collisionMap.itemIndex].isActive = false;
                    itemManager.items[collisionMap.itemIndex].position.x = 0;
                    itemManager.items[collisionMap.itemIndex].position.y = 0;
                }
            }
        }


        public void Draw()
        {
            // used to draw the player
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(avatar);
            gameMap.SetColorDefault();
        }


        public void TriggerShop(string type)
        {
            shopping = true;
            shop.OpenShop(type);
        }


        public void GetPlayerInput(Map collisionMap)
        {
            int moveX;
            int moveY;
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            playerInput = Console.ReadKey(true);
            {
                if (shopping) { return; }

                if (playerInput.Key == ConsoleKey.W || playerInput.Key == ConsoleKey.UpArrow && !shopping)
                {
                    moveY = (position.y - 1);
                    if (moveY <= 0)
                    {
                        moveY = 0; 
                    }
                    if (collisionMap.CreatureInTarget(moveY, position.x) && collisionMap.index != 0) // Player should always be 0, need to prevent self harm.
                    {
                        collisionMap.characters[collisionMap.index].healthSystem.TakeDamage(playerDamage);
                        enemyHitName = collisionMap.characters[collisionMap.index].name;
                        enemyHitHealth = collisionMap.characters[collisionMap.index].healthSystem.health;
                        enemyHitArmor = collisionMap.characters[collisionMap.index].healthSystem.armour;

                        if (collisionMap.characters[collisionMap.index].healthSystem.health <= 0)
                        {
                            Debug.WriteLine(enemyHitName);
                            shop.ClaimBountyOn(enemyHitName);
                        }
                        
                        moveY = position.y;
                        position.y = moveY;
                        Debug.WriteLine("Player Hit " + enemyHitName);
                        return;
                    }
                    if (collisionMap.ItemInTarget(moveY, position.x) && itemManager.items[collisionMap.itemIndex].isActive)
                    {
                        ItemCheck(collisionMap);
                    }
                    if (collisionMap.CheckTile(moveY, position.x) == false)
                    {
                        moveY = position.y;
                        position.y = moveY;
                        return;
                    }
                    else // You're not allowed to move there
                    {
                        position.y = moveY;
                        if (position.y <= 0)
                        {
                            position.y = 0;
                        }
                    }
                }
                if (playerInput.Key == ConsoleKey.S || playerInput.Key == ConsoleKey.DownArrow && !shopping)
                {
                    moveY = (position.y + 1);
                    if (moveY >= position.maxY)
                    {
                        moveY = position.maxY;
                    }
                    if (collisionMap.CreatureInTarget(moveY, position.x) && collisionMap.index != 0)
                    {
                        collisionMap.characters[collisionMap.index].healthSystem.TakeDamage(playerDamage);
                        enemyHitName = collisionMap.characters[collisionMap.index].name;
                        enemyHitHealth = collisionMap.characters[collisionMap.index].healthSystem.health;
                        enemyHitArmor = collisionMap.characters[collisionMap.index].healthSystem.armour;
                        if (collisionMap.characters[collisionMap.index].healthSystem.health <= 0)
                        {
                            Debug.WriteLine(enemyHitName);
                            shop.ClaimBountyOn(enemyHitName);
                        }
                        moveY = position.y;
                        position.y = moveY;
                        Debug.WriteLine("Player Hit " + enemyHitName);
                        return;
                    }
                    if (collisionMap.ItemInTarget(moveY, position.x) && itemManager.items[collisionMap.itemIndex].isActive && !shopping)
                    {
                        ItemCheck(collisionMap);
                    }
                    if (collisionMap.CheckTile(moveY, position.x) == false)
                    {
                        moveY = position.y;
                        position.y = moveY;
                        return;
                    }
                    else // You're not allowed to move there
                    {
                        position.y = moveY;
                        if (position.y >= position.maxY)
                        {
                            position.y = position.maxY;
                        }
                    }
                }
                if (playerInput.Key == ConsoleKey.A || playerInput.Key == ConsoleKey.LeftArrow && !shopping)
                {
                    //Moves player left
                    moveX = (position.x - 1);
                    if (moveX <= 0)
                    {
                        moveX = 0; 
                    }
                    if (collisionMap.CreatureInTarget(position.y, moveX) && collisionMap.index != 0)
                    {
                        collisionMap.characters[collisionMap.index].healthSystem.TakeDamage(playerDamage);
                        enemyHitName = collisionMap.characters[collisionMap.index].name;
                        enemyHitHealth = collisionMap.characters[collisionMap.index].healthSystem.health;
                        enemyHitArmor = collisionMap.characters[collisionMap.index].healthSystem.armour;

                        if (collisionMap.characters[collisionMap.index].healthSystem.health <= 0)
                        {
                            Debug.WriteLine(enemyHitName);
                            shop.ClaimBountyOn(enemyHitName);
                        }

                        moveX = position.x;
                        position.x = moveX;
                        Debug.WriteLine("Player Hit " + enemyHitName);
                        return;
                    }
                    if (collisionMap.ItemInTarget(position.y, moveX) && itemManager.items[collisionMap.itemIndex].isActive)
                    {
                        ItemCheck(collisionMap);
                    }
                    if (collisionMap.CheckTile(position.y, moveX) == false)
                    {
                        moveX = position.x;
                        position.x = moveX;
                        return;
                    }
                    else // You're not allowed to move there
                    {
                        position.x = moveX;
                        if (position.x <= 0)
                        {
                            position.x = 0;
                        }
                    }
                }
                if (playerInput.Key == ConsoleKey.D || playerInput.Key == ConsoleKey.RightArrow && !shopping)
                {
                    moveX = (position.x + 1);
                    if (moveX >= position.maxX)
                    {
                        moveX = position.maxX;
                    }
                    if (collisionMap.CreatureInTarget(position.y, moveX) && collisionMap.index != 0)
                    {
                        collisionMap.characters[collisionMap.index].healthSystem.TakeDamage(playerDamage);
                        enemyHitName = collisionMap.characters[collisionMap.index].name;
                        enemyHitHealth = collisionMap.characters[collisionMap.index].healthSystem.health;
                        enemyHitArmor = collisionMap.characters[collisionMap.index].healthSystem.armour;

                        if (collisionMap.characters[collisionMap.index].healthSystem.health <= 0)
                        {
                            Debug.WriteLine(enemyHitName);
                            shop.ClaimBountyOn(enemyHitName);
                        }

                        moveX = position.x;
                        position.x = moveX;
                        Debug.WriteLine("Player Hit " + enemyHitName);
                        return;
                    }
                    if (collisionMap.ItemInTarget(position.y, moveX) && itemManager.items[collisionMap.itemIndex].isActive)
                    {
                        ItemCheck(collisionMap);
                    }
                    if (collisionMap.CheckTile(position.y, moveX) == false)
                    {
                        moveX = position.x;
                        position.x = moveX;
                        return;
                    }
                    else // You're not allowed to move there
                    {
                        position.x = moveX;
                        if (position.x >= position.maxX)
                        {
                            position.x = position.maxX;
                        }
                    }
                }
                if (collisionMap.activeMap[position.y, position.x] == '$')
                {
                    //ends game when touching the "Grail"
                    gameWon = true;
                    gameIsOver = true;
                }
                if (collisionMap.activeMap[position.y, position.x] == '~')
                {
                    //Advances to next level
                    collisionMap.levelNumber += 1;
                    collisionMap.ChangeLevels();
                    SetPlayerPosition(collisionMap.playerX, collisionMap.playerY);
                }
                if (collisionMap.activeMap[position.y, position.x] == '*')
                {
                    // this is a hazard
                    healthSystem.health -= 1;
                }
                if (playerInput.Key == ConsoleKey.Escape)
                {
                    //leaves game
                    Environment.Exit(0);
                }
                }
            }
            
        }
    }
