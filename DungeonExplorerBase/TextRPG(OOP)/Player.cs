using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Is the player, Handles all player movment and interactions.
    /// </summary>
    internal class Player : Character
    {
        public int playerDamage;
        public int playerDamageUps;
        public int PlayerMaxHP;
        public ConsoleKeyInfo playerInput;
        public bool gameIsOver;
        public bool gameWon;
        public bool shopping;
        public string enemyHitName;
        public int enemyHitHealth;
        public int enemyHitArmor;
        public int StartingDamage;
        public Map gameMap;
        public char avatar;
        public ItemManager itemManager;
        public ShopManager shop;
        public QuestManager quest;
        public Player(Map map, ItemManager IM, Settings settings, ShopManager shop, QuestManager quest)
        {
            avatar = ((char)2); // Sets player to smiley face.
            healthSystem.IsAlive = true; // initilizes player as alive.
            gameIsOver = false;
            gameWon = false;
            StartingDamage = settings.PlayerBaseDamage; // Sets player starting damage
            playerDamage = StartingDamage + playerDamageUps;
            PlayerMaxHP = settings.playerMaxHP; // Sets stating health
            healthSystem.SetHealth(PlayerMaxHP);// hands starting value to health system
            name = "Player"; // Testing for passing string.
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
        }
        /// <summary>
        /// Gets player input and updates player based on interactions. 
        /// </summary>
        public void Update()
        {
            GetPlayerInput(gameMap);
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
        /// <summary>
        /// Sets player position to x/y postions. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>

        public void ItemCheck(Map collisionMap)
        {
            if (itemManager.items[collisionMap.itemIndex].itemType == "Health Pickup" && healthSystem.health < PlayerMaxHP)
            {
                GoShopping();
                if (shopping)
                {
                    return;
                }
                else if (!shopping) 
                {
                    itemManager.items[collisionMap.itemIndex].isActive = false;
                    itemManager.items[collisionMap.itemIndex].position.x = 0;
                    itemManager.items[collisionMap.itemIndex].position.y = 0;
                    gameMap.UpdateHealthUIInfo();
                    healthSystem.Heal(itemManager.items[collisionMap.itemIndex].gainAmount, PlayerMaxHP);
                }
            }
            if (itemManager.items[collisionMap.itemIndex].itemType == "Armor Pickup")
            {
                GoShopping();
                Debug.WriteLine(shopping);
                if (shopping)
                {
                    return;
                }
                else if (!shopping)
                {
                    itemManager.items[collisionMap.itemIndex].isActive = false;
                    itemManager.items[collisionMap.itemIndex].position.x = 0;
                    itemManager.items[collisionMap.itemIndex].position.y = 0;
                    gameMap.UpdateArmorUIInfo();
                    healthSystem.armor += itemManager.items[collisionMap.itemIndex].gainAmount;
                }
            }
            if (itemManager.items[collisionMap.itemIndex].itemType == "Damage Pickup")
            {
                GoShopping();
                if (shopping)
                {
                    return;
                }
                else if (!shopping)
                { 
                    itemManager.items[collisionMap.itemIndex].isActive = false;
                    itemManager.items[collisionMap.itemIndex].position.x = 0;
                    itemManager.items[collisionMap.itemIndex].position.y = 0;
                    playerDamageUps += itemManager.items[collisionMap.itemIndex].gainAmount;
                    gameMap.UpdateDamageUIInfo();
                    playerDamageUps++;
                }
            }
        }

        public void SetPlayerPosition(int x, int y)
        {
            position.x = x;
            position.y = y;
        }

        public void GoShopping()
        {
            gameMap.ShopMenu();
            Debug.WriteLine("Am I shopping? " + shopping);
        }

        public void ExitShop()
        { 
        
        }



        /// <summary>
        /// Input method, needs map to check for collision
        /// </summary>
        /// <param name="collisionMap"></param>
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
                if (playerInput.Key == ConsoleKey.Q || playerInput.Key == ConsoleKey.E)
                { 
                
                }
                if (shopping) { return; }
                if(playerInput.Key == ConsoleKey.W || playerInput.Key == ConsoleKey.UpArrow)
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
                        enemyHitArmor = collisionMap.characters[collisionMap.index].healthSystem.armor;
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
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Health Pickup" && healthSystem.health < PlayerMaxHP)
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    gameMap.UpdateHealthUIInfo();
                        //    healthSystem.Heal(itemManager.items[collisionMap.itemIndex].gainAmount, PlayerMaxHP);
                        //}
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Armor Pickup")
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    gameMap.UpdateArmorUIInfo();
                        //    healthSystem.armor += itemManager.items[collisionMap.itemIndex].gainAmount;
                        //}
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Damage Pickup")
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    playerDamageUps += itemManager.items[collisionMap.itemIndex].gainAmount;
                        //    gameMap.UpdateDamageUIInfo();
                        //    playerDamage = playerDamageUps + 1;
                        //}
                    }
                    if(collisionMap.CheckTile(moveY, position.x) == false)
                    {
                        moveY = position.y;
                        position.y = moveY;
                        return;
                    }
                    else
                    {
                        position.y = moveY;
                        if(position.y <= 0)
                        {
                            position.y = 0;
                        }
                    }
                }
                if(playerInput.Key == ConsoleKey.S || playerInput.Key == ConsoleKey.DownArrow)
                {
                    //Moves player down
                    moveY = (position.y + 1);
                    if(moveY >= position.maxY)
                    {
                        moveY = position.maxY; //Locks top of screen
                    }
                    if(collisionMap.CreatureInTarget(moveY, position.x) && collisionMap.index != 0)
                    {
                        collisionMap.characters[collisionMap.index].healthSystem.TakeDamage(playerDamage);
                        enemyHitName = collisionMap.characters[collisionMap.index].name;
                        enemyHitHealth = collisionMap.characters[collisionMap.index].healthSystem.health;
                        enemyHitArmor = collisionMap.characters[collisionMap.index].healthSystem.armor;
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
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Health Pickup" && healthSystem.health < PlayerMaxHP)
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    healthSystem.Heal(itemManager.items[collisionMap.itemIndex].gainAmount, PlayerMaxHP);
                        //}
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Armor Pickup")
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    healthSystem.armor += itemManager.items[collisionMap.itemIndex].gainAmount;
                        //}
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Damage Pickup")
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    playerDamageUps += itemManager.items[collisionMap.itemIndex].gainAmount;
                        //    playerDamage = playerDamageUps + 1;
                        //}
                    }
                    if(collisionMap.CheckTile(moveY, position.x) == false)
                    {
                        moveY = position.y;
                        position.y = moveY;
                        return;
                    }
                    else
                    {
                        position.y = moveY;
                        if(position.y >= position.maxY)
                        {
                            position.y = position.maxY;
                        }
                    }
                }
                if(playerInput.Key == ConsoleKey.A || playerInput.Key == ConsoleKey.LeftArrow)
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
                        enemyHitArmor = collisionMap.characters[collisionMap.index].healthSystem.armor;
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
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Health Pickup" && healthSystem.health < PlayerMaxHP)
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    healthSystem.Heal(itemManager.items[collisionMap.itemIndex].gainAmount, PlayerMaxHP);
                        //}
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Armor Pickup")
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    healthSystem.armor += itemManager.items[collisionMap.itemIndex].gainAmount;
                        //}
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Damage Pickup")
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    playerDamageUps += itemManager.items[collisionMap.itemIndex].gainAmount;
                        //    playerDamage = playerDamageUps + 1;
                        //}
                    }
                    if(collisionMap.CheckTile(position.y, moveX) == false)
                    {
                        moveX = position.x;
                        position.x = moveX;
                        return;
                    }
                    else
                    {
                        position.x = moveX;
                        if(position.x <= 0)
                        {
                            position.x = 0;
                        }
                    }
                }
                if(playerInput.Key == ConsoleKey.D || playerInput.Key == ConsoleKey.RightArrow)
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
                        enemyHitArmor = collisionMap.characters[collisionMap.index].healthSystem.armor;
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
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Health Pickup" && healthSystem.health < PlayerMaxHP)
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    healthSystem.Heal(itemManager.items[collisionMap.itemIndex].gainAmount, PlayerMaxHP);
                        //}
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Armor Pickup")
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    healthSystem.armor += itemManager.items[collisionMap.itemIndex].gainAmount;
                        //}
                        //if(itemManager.items[collisionMap.itemIndex].itemType == "Damage Pickup")
                        //{
                        //    itemManager.items[collisionMap.itemIndex].isActive = false;
                        //    itemManager.items[collisionMap.itemIndex].position.x = 0;
                        //    itemManager.items[collisionMap.itemIndex].position.y = 0;
                        //    playerDamageUps += itemManager.items[collisionMap.itemIndex].gainAmount;
                        //    playerDamage = playerDamageUps + 1;
                        //}
                    }
                    if(collisionMap.CheckTile(position.y, moveX) == false)
                    {
                        moveX = position.x;
                        position.x = moveX;
                        return;
                    }
                    else
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
        /// Used to increase player stats based on collions.
        /// </summary>
        void CheckPlayerWallet() // change to call to ShopManager 
        {
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
