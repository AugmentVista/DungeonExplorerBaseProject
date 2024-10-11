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

        public int playerArmourUps;
        public int playerHealthUps;
        public int playerDamageUps;

        public int enemyHitHealth;
        public int enemyHitArmor;
        public int StartingDamage = 1;

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
            playerDamage = StartingDamage;
            healthSystem.SetHealth(Settings.playerMaxHP);// hands starting value to health system
            name = "Player";
            enemyHitName = ""; // clears enemy hit for starting
            gameMap = map; // hands map to player
            itemManager = IM; // hands item manager to player
            this.shop = shop;
            shop.SetPlayer(this);
            this.quest = quest;
            quest.SetPlayer(this);
        }
        /// <summary>
        /// Used at start to prevent player from leaving screen.
        /// </summary>
        public void Start()
        {
            SetMaxPlayerPosition(gameMap);
            playerHealth = Settings.playerMaxHP;
        }
        public void Update()
        {
            playerHealth = healthSystem.GetHealth();
            playerDamage = StartingDamage + playerDamageUps;
            playerArmour = playerArmourUps;
            CapPlayerStats();
            GetPlayerInput(gameMap);
        }

        private void CapPlayerStats()
        {
            if (playerHealth > Settings.playerMaxHP) { playerHealth = Settings.playerMaxHP; }
            if (playerArmour > Settings.playerMaxArmour) { playerArmour = Settings.playerMaxArmour; }
            if (playerDamage > Settings.playerMaxDamage) { playerDamage = Settings.playerMaxDamage; }
        }

        /// <summary>
        /// used to keep player in map
        /// </summary>
        /// <param name="map"></param>
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
        /// <summary>
        /// Sets player position to x/y postions. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>

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
                    healthSystem.Heal(itemManager.items[collisionMap.itemIndex].gainAmount, Settings.playerMaxHP);
                    gameMap.UpdateHealthUIInfo();
                }
                // maybe else needed
            }
            if (itemManager.items[collisionMap.itemIndex].itemType == "Armor Pickup")
            {
                TriggerShop("Armour");
                if (itemManager.items[collisionMap.itemIndex].isActive != false)
                {
                    itemManager.items[collisionMap.itemIndex].isActive = false;
                    itemManager.items[collisionMap.itemIndex].position.x = 0;
                    itemManager.items[collisionMap.itemIndex].position.y = 0;
                    healthSystem.armour += itemManager.items[collisionMap.itemIndex].gainAmount;
                    gameMap.UpdateArmorUIInfo();
                }
            }
            if (itemManager.items[collisionMap.itemIndex].itemType == "Damage Pickup")
            {
                TriggerShop("Damage");
                if (itemManager.items[collisionMap.itemIndex].isActive != false)
                {
                    itemManager.items[collisionMap.itemIndex].isActive = false;
                    itemManager.items[collisionMap.itemIndex].position.x = 0;
                    itemManager.items[collisionMap.itemIndex].position.y = 0;
                    playerDamageUps += itemManager.items[collisionMap.itemIndex].gainAmount;
                    playerDamageUps++;
                    gameMap.UpdateDamageUIInfo();
                }
            }
        }

        public void TriggerShop(string type)
        {
            shopping = true;
            shop.OpenShop(type);
        }
        public void GetPlayerInput(Map collisionMap) // use this to break out of shop
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

                if(playerInput.Key == ConsoleKey.W || playerInput.Key == ConsoleKey.UpArrow && !shopping)
                {
                    moveY = (position.y - 1);
                    if(moveY <= 0)
                    {
                        moveY = 0; //Locks top of screen
                    }
                    if(collisionMap.CreatureInTarget(moveY, position.x) && collisionMap.index != 0) // Player should always be 0, need to prevent self harm.
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
                    if(collisionMap.ItemInTarget(moveY, position.x) && itemManager.items[collisionMap.itemIndex].isActive)
                    {
                        ItemCheck(collisionMap);
                    }
                    if(collisionMap.CheckTile(moveY, position.x) == false)
                    {
                        moveY = position.y;
                        position.y = moveY;
                        return;
                    }
                    else // You're not allowed to move there
                    {
                        position.y = moveY;
                        if(position.y <= 0)
                        {
                            position.y = 0;
                        }
                    }
                }
                if(playerInput.Key == ConsoleKey.S || playerInput.Key == ConsoleKey.DownArrow && !shopping)
                {
                    moveY = (position.y + 1);
                    if(moveY >= position.maxY)
                    {
                        moveY = position.maxY;
                    }
                    if(collisionMap.CreatureInTarget(moveY, position.x) && collisionMap.index != 0)
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
                    if(collisionMap.ItemInTarget(moveY, position.x) && itemManager.items[collisionMap.itemIndex].isActive && !shopping)
                    {
                        ItemCheck(collisionMap);
                    }
                    if(collisionMap.CheckTile(moveY, position.x) == false)
                    {
                        moveY = position.y;
                        position.y = moveY;
                        return;
                    }
                    else // You're not allowed to move there
                    {
                        position.y = moveY;
                        if(position.y >= position.maxY)
                        {
                            position.y = position.maxY;
                        }
                    }
                }
                if(playerInput.Key == ConsoleKey.A || playerInput.Key == ConsoleKey.LeftArrow && !shopping)
                {
                    //Moves player left
                    moveX = (position.x - 1);
                    if(moveX <= 0)
                    {
                        moveX = 0; //Locks top of screen
                    }
                    if(collisionMap.CreatureInTarget(position.y, moveX) && collisionMap.index != 0)
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
                    if(collisionMap.ItemInTarget(position.y, moveX) && itemManager.items[collisionMap.itemIndex].isActive)
                    {
                        ItemCheck(collisionMap);
                    }
                    if(collisionMap.CheckTile(position.y, moveX) == false)
                    {
                        moveX = position.x;
                        position.x = moveX;
                        return;
                    }
                    else // You're not allowed to move there
                    {
                        position.x = moveX;
                        if(position.x <= 0)
                        {
                            position.x = 0;
                        }
                    }
                }
                if(playerInput.Key == ConsoleKey.D || playerInput.Key == ConsoleKey.RightArrow && !shopping )
                {
                    moveX = (position.x + 1);
                    if(moveX >= position.maxX)
                    {
                        moveX = position.maxX; //Locks top of screen
                    }
                    if(collisionMap.CreatureInTarget(position.y, moveX) && collisionMap.index != 0)
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
                    if(collisionMap.ItemInTarget(position.y, moveX) && itemManager.items[collisionMap.itemIndex].isActive)
                    {
                        ItemCheck(collisionMap);
                    }
                    if(collisionMap.CheckTile(position.y, moveX) == false)
                    {
                        moveX = position.x;
                        position.x = moveX;
                        return;
                    }
                    else // You're not allowed to move there
                    {
                        position.x = moveX;
                        if(position.x >= position.maxX)
                        {
                            position.x = position.maxX;
                        }
                    }
                }
                if(collisionMap.activeMap[position.y,position.x] == '$')
                {
                    //ends game when touching the "Grail"
                    gameWon = true;
                    gameIsOver = true;
                }
                if(collisionMap.activeMap[position.y,position.x] == '~')
                {
                    //Advances to next level
                    collisionMap.levelNumber += 1;
                    collisionMap.ChangeLevels();
                    SetPlayerPosition(collisionMap.playerX,collisionMap.playerY);
                }
                if(collisionMap.activeMap[position.y,position.x] == '*')
                {
                    // this is a hazard
                    healthSystem.health -= 1;
                }
                if(playerInput.Key == ConsoleKey.Escape)
                {
                    //leaves game
                    Environment.Exit(0);
                }
            }
        }
        /// <summary>
        /// Draws player to map.
        /// </summary>
        public void Draw()
        {
            // used to draw the player
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(position.x,position.y);
            Console.Write(avatar);
            gameMap.SetColorDefault();
        }
    }
}
