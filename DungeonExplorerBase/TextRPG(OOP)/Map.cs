﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Collections;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Build and manages the game map
    /// </summary>
    internal class Map
    {
        public static string path;
        public static string path1 = @"Floor1Map.txt";
        public static string path2 = @"Floor2Map.txt";
        public static string path3 = @"Floor3Map.txt";
        public static string[] floorMap;
        public char[,] activeMap;
        public int levelNumber;
        public bool levelChanged;
        public char dungeonFloor = ((char)18); // ↕
        public char dungeonWall = ((char)35); // #
        public char spikeTrap = ((char)23); // ↨
        public char stairsDown = ((char)30); // ▲
        public char startPos = ((char)31); // ▼
        public char finalLoot = ((char)165); // ¥
        public char damagePickup = ((char)164); // ¤
        public char healthPickup = ((char)3); // ♥
        public char armorPickup = ((char)21); // §
        static char enemy1 = ((char)4); // ? = Construct
        static char enemy2 = ((char)6); // ! = Plasmoid
        static char enemy3 = ((char)5); // & = GoblinFolk

        public static int mapX;
        public static int mapY;
        public int playerX;
        public int playerY;
        public int playerMaxX;
        public int playerMaxY;
        public List<Character> characters;
        public int enemyCount;
        public int itemCount;
        public int index;
        public int itemIndex;


        public EnemyManager enemyManager;
        public Player mainPlayer;
        public ItemManager itemManager;
        public QuestManager questManager;
        public Map(ItemManager IM, QuestManager quest) //Constructor
        {
            questManager = quest;
            Initialization();
            //Sets item manager from call in GameManager
            itemManager = IM;
            quest.SetMap(this);
        }
        /// <summary>
        /// Starts the map building process.
        /// </summary>
        public void Initialization()
        {
            enemyCount = 0;
            levelNumber = 1;
            path = path1;
            characters = new List<Character>();
            floorMap = File.ReadAllLines(path);
            activeMap = new char[floorMap.Length, floorMap[0].Length];
            mapX = activeMap.GetLength(1);
            mapY = activeMap.GetLength(0);
            MakeDungeonMap();
            index = 0;
            itemIndex = 0;
            ///damageUpgradeCount = mainPlayer.
            ///healthUpgradeCount
           ///armourUpgradeCount
    }
        /// <summary>
        /// Sets player and Enemy manager from call in gameManager.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="em"></param>
        public void Start(Player player, EnemyManager em)
        {
            enemyManager = em;
            mainPlayer = player;
            AddToCharacterList(mainPlayer);
            SetPlayerSpawn(mainPlayer);
            GetPlayerMaxPosition(mainPlayer);
            UpdateAllUI(); // displays the starting prices for upgrades
        }
        /// <summary>
        /// Creates map based on active floor.
        /// </summary>
        public void MakeDungeonMap()
        {
            for (int i = 0; i < floorMap.Length; i++)
            {
                for (int j = 0; j < floorMap[i].Length; j++)
                {
                    activeMap[i, j] = floorMap[i][j];
                }
            } 
        }
        /// <summary>
        /// Updates values needed for map.
        /// </summary>
        public void Update()
        {
            index = 0;
            itemIndex = 0;
        }
        /// <summary>
        /// Calls function to draw map/Hud/Legend
        /// </summary>
        public void Draw()
        {
            DrawMap();
            DrawHUD();
            DrawEnemyLegend();
            UpdateAllUI();
        }
        /// <summary>
        /// Draws the map of the current level
        /// </summary>
        public void DrawMap()
        {
            Console.SetCursorPosition(0,0);
            for(int y = 0; y < mapY; y++)
            {
                for(int x = 0; x < mapX; x++)
                {
                    char tile = activeMap[y,x];
                    if(tile == '=' && levelChanged == false)
                    {
                        playerX = x;
                        playerY = y-1;
                        levelChanged = true;
                        activeMap[y,x] = '#';
                        mainPlayer.SetPlayerPosition(playerX,playerY);
                    }
                    if(tile == '!' && levelChanged == false || tile == '?' && levelChanged == false
                    || tile == '&' && levelChanged == false || tile == '^' && levelChanged == false)
                    {
                        if(tile == '!')
                        {
                            enemyManager.AddEnemiesToList("Plasmoid", levelNumber);
                            enemyManager.enemiesList[enemyCount].position.x = x;
                            enemyManager.enemiesList[enemyCount].position.y = y;
                            AddToCharacterList(enemyManager.enemiesList[enemyCount]);
                            activeMap[y, x] = '-';
                            enemyCount += 1;
                        }
                        if(tile == '?')
                        {
                            enemyManager.AddEnemiesToList("Construct", levelNumber);
                            enemyManager.enemiesList[enemyCount].position.x = x;
                            enemyManager.enemiesList[enemyCount].position.y = y;
                            AddToCharacterList(enemyManager.enemiesList[enemyCount]);
                            activeMap[y, x] = '-';
                            enemyCount += 1;
                        }
                        if(tile == '&')
                        {
                            enemyManager.AddEnemiesToList("GoblinFolk", levelNumber);
                            enemyManager.enemiesList[enemyCount].position.x = x;
                            enemyManager.enemiesList[enemyCount].position.y = y;
                            AddToCharacterList(enemyManager.enemiesList[enemyCount]);
                            activeMap[y, x] = '-';
                            enemyCount += 1;
                        }
                    }
                    if(tile == '@' && levelChanged == false)
                    {
                        itemManager.AddItemToList("Damage Pickup",x,y);
                        itemManager.items[itemCount].index = itemCount;
                        itemCount += 1;
                    }
                    if(tile == '"' && levelChanged == false)
                    {
                        itemManager.AddItemToList("Health Pickup",x,y);
                        itemManager.items[itemCount].index = itemCount;
                        itemCount += 1;
                    }
                    if(tile == '+' && levelChanged == false)
                    {
                        itemManager.AddItemToList("Armor Pickup",x,y);
                        itemManager.items[itemCount].index = itemCount;
                        itemCount += 1;
                    }
                    DrawTile(tile);
                }
                Console.Write("\n");
            }
        }
        /// <summary>
        /// Sets player spawn point
        /// </summary>
        /// <param name="player"></param>
        public void SetPlayerSpawn(Character player)
        {
            player.position.x = playerX;
            player.position.y = playerY;
        }
        /// <summary>
        /// Draws the player at current position.
        /// </summary>
        /// <param name="player"></param>
        public void GetPlayerMaxPosition(Player player)
        {
            playerMaxX = player.position.maxX;
            playerMaxY = player.position.maxY;
        }
        /// <summary>
        /// Draws enemys to map based on enemy position
        /// </summary>
        /// <param name="eList"></param>
        public void DrawEnemiesToMap(List<Enemy> eList)
        {
            for(int i = 0; i < eList.Count(); i++)
            {
                if(eList[i].healthSystem.IsAlive == true)
                {
                    Console.SetCursorPosition(eList[i].position.x,eList[i].position.y);
                    DrawEnemy(eList[i]);
                }
            }
        }
        /// <summary>
        /// Makes sure enemy matches avatar and color
        /// </summary>
        /// <param name="enemy"></param>
        public void DrawEnemy(Enemy enemy)
        {
             
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = enemy.avatarColor;
            Console.Write(enemy.avatar);
            SetColorDefault();
        }
        /// <summary>
        /// Code for tiles color and ascii
        /// </summary>
        public void DrawFloor()
        {
            // used to draw a floor tile
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(dungeonFloor);
            SetColorDefault();
        }
        /// <summary>
        /// Code for tiles color and ascii
        /// </summary>
        public void DrawWall()
        {
            // used to draw a wall tile
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write(dungeonWall);
            SetColorDefault();
        }
        /// <summary>
        /// Code for tiles color and ascii
        /// </summary>
        public void DrawSpikes()
        {
            // used to draw a spikes tile
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(spikeTrap);
            SetColorDefault();
        }
        /// <summary>
        /// Code for tiles color and ascii
        /// </summary>
        public void DrawFinalLoot()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(finalLoot);
            SetColorDefault();
        }
        /// <summary>
        /// Code for tiles color and ascii
        /// </summary>
        public void DrawDamageUpgrade()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(damagePickup);
            SetColorDefault();
        }
        /// <summary>
        /// Code for tiles color and ascii
        /// </summary>
        public void SetColorDefault()
        {
            // sets console color back to default. 
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        /// <summary>
        /// Code for tiles color and ascii
        /// </summary>
        public void DrawArmor()
        {
            //Draws armor tile to map
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(armorPickup);
            SetColorDefault();
        }
        /// <summary>
        /// Code for tiles color and ascii
        /// </summary>
        public void DrawStairsDown()
        {
            //Draws stairs to next level
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(stairsDown);
            SetColorDefault();
        }
        /// <summary>
        /// Code for tiles color and ascii
        /// </summary>
        public void DrawHealthPickup()
        {
            //Draws health pickup
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(healthPickup);
            SetColorDefault();
        }
        /// <summary>
        /// Determines what tile is what during print.
        /// </summary>
        /// <param name="tile"></param>
        public void DrawTile(Char tile)
        {
            // draws the correct tile based on the floorMap
            if(tile == '-')
            {
                DrawFloor();
                return;
            }
            if(tile == '#')
            {
                DrawWall();
                return;
            }
            if(tile == '*')
            {
                DrawSpikes();
                return;
            }
            if(tile == '~')
            {
                DrawStairsDown();
                return;
            }
            if(tile == '=')
            {
                DrawWall();
                return;
            }
            if(tile == '$')
            {
                DrawFinalLoot();
                return;
            }
            if(tile == '@')
            {
                DrawFloor();
                return;
            }
            if(tile == '"')
            {
                DrawFloor();
                return;
            }
            if(tile == '!')
            {
                DrawFloor();
                return;
            }
            if(tile == '?')
            {
                DrawFloor();
                return;
            }
            if(tile == '&')
            {
                DrawFloor();
                return;
            }
            if(tile == '^')
            {
                DrawFloor();
                return;
            }
            if(tile == '+')
            {
                DrawFloor();
                return;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(tile);
            }
        }
        /// <summary>
        /// Changes level based on level number
        /// </summary>
        public void ChangeLevels()
        {
            levelChanged = false;
            // used to change maps
            if(levelNumber == 1)
            {
                path = path1;
                floorMap = File.ReadAllLines(path);
            }
            if(levelNumber == 2)
            {
                levelNumber = 2;
                path = path2;
                floorMap = File.ReadAllLines(path);
                if (QuestManager.questsCompleted < 3) { QuestManager.questsCompleted = 3; }
            }
            if(levelNumber == 3)
            {
                levelNumber = 3;
                path = path3;
                floorMap = File.ReadAllLines(path);
                if (QuestManager.questsCompleted < 5) { QuestManager.questsCompleted = 5; }
            }
            if(levelNumber > 3 || levelNumber <= 0)
            {
                Console.Clear();
                Console.WriteLine("Level Out of range, Loading level 1");
                path = path1;
                floorMap = File.ReadAllLines(path);
            }
            RestCharacters();
            ResetItems();
            MakeDungeonMap();
            DrawMap();
        }
        /// <summary>
        /// Checks to see if any creatre is in the checked x/y position
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool CreatureInTarget(int y, int x)
        {
            //Used to check if a creture is in target location. 
            bool IsTarget = false;
            for(index = 0; index < characters.Count(); index++)
            {
                if(x == characters[index].position.x && y == characters[index].position.y)
                {
                    IsTarget = true;
                    return IsTarget;
                }
                if(index > characters.Count() || index < 0)
                {
                    index = 0;
                }
            }
            return IsTarget;
        }
        /// <summary>
        /// Checks if player is in the checked x/y position
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool IsPlayerInTarget(int y, int x)
        {
            //Checks if player is in target loctaion
            bool IsTarget;
            if(mainPlayer.position.y == y && mainPlayer.position.x == x)
            {
                IsTarget = true;
            }
            else
            {
                IsTarget = false;
            }
            return IsTarget;
        }
        /// <summary>
        /// Checks target x/y position for an item
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool ItemInTarget(int y, int x)
        {
            //Checks for Item in target location
            bool IsTarget = false;
            for(itemIndex = 0; itemIndex < itemManager.items.Count(); itemIndex++)
            {
                if(x == itemManager.items[itemIndex].position.x && y == itemManager.items[itemIndex].position.y)
                {
                    IsTarget = true;
                    return IsTarget;
                }
                if(itemIndex > itemManager.items.Count() || itemIndex < 0)
                {
                    index = 0;
                }
            }
            return IsTarget;
        }
        /// <summary>
        /// Adds character to active characters list, used for collision checks. 
        /// </summary>
        /// <param name="character"></param>
        public void AddToCharacterList(Character character)
        {
            //Adds to list for character used for the floor. 
            characters.Add(character);
        }
        /// <summary>
        /// Checks to make sure tile is walkable.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool CheckTile(int y, int x)
        {
            //Makes sure base tile is walkable. 
            bool CanMove = false;
            if(activeMap[y,x] == dungeonWall)
            {
                CanMove = false;
            }
            else
            {
                CanMove = true;
            }
            return CanMove;
        }
        /// <summary>
        /// Resets characters for map change.
        /// </summary>
        public void RestCharacters()
        {
            //Resets map for moving floors.
            enemyManager.enemiesList.Clear();
            characters.Clear();
            enemyCount = 0;
            characters.Add(mainPlayer);
        }
        /// <summary>
        /// Resets items for map change
        /// </summary>
        public void ResetItems()
        {
            //resets items for moving floors.
            itemManager.items.Clear();
            itemCount = 0;
        }
        /// <summary>
        /// Draws legend to screen outside of the game map
        /// </summary>
        public void DrawEnemyLegend()
        {
            Console.SetCursorPosition(mapX + 1, 3);
            Console.Write(enemy2);
            Console.Write(" = Plasmoid");
            Console.SetCursorPosition(mapX + 1, 4);
            Console.Write(enemy1);
            Console.Write(" = Construct");
            Console.SetCursorPosition(mapX + 1, 5);
            Console.Write(enemy3);
            Console.Write(" = Goblin Folk");
        }

        public void UpdateAllUI()
        {
            UpdateArmorUIInfo("Display price");
            UpdateHealthUIInfo("Display price");
            UpdateDamageUIInfo("Display price");
            UpdateQuestUIInfo();
        }

        public void UpdateQuestUIInfo()
        {
            questManager.UpdateActiveQuest();
            Console.SetCursorPosition(mapX + 1, 15);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Quests to be completed: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            int lineOffset = 1;
            foreach (var quest in questManager.questsOrder)
            {
                Console.SetCursorPosition(mapX + 1, 16 + lineOffset);
                Console.Write(quest);
                Console.WriteLine();
                lineOffset++;
            }
        }

        public void UpdateArmorUIInfo(string type)
        {
            if (type == "Armour")
            {
                Console.SetCursorPosition(mapX + 1, 7);
                Console.Write(armorPickup);
                Console.Write(" = Armor Item Shop");

                mainPlayer.shop.UpdateUpgradeCosts(type); // every call doubles price

                Console.SetCursorPosition(mapX + 1, 8);
                Console.Write(" Current market price: " + mainPlayer.shop.armourUpgradeCost);
            }
            else
                Console.SetCursorPosition(mapX + 1, 7);
                Console.Write(armorPickup);
                Console.Write(" = Armor Item Shop");
                Console.SetCursorPosition(mapX + 1, 8);
                Console.Write(" Current market price: " + mainPlayer.shop.armourUpgradeCost);
        }

        public void UpdateHealthUIInfo(string type)
        {
            if (type == "Health")
            {
                Console.SetCursorPosition(mapX + 1, 9);
                Console.Write(healthPickup);
                Console.Write(" Health Item Shop ");

                mainPlayer.shop.UpdateUpgradeCosts(type); // every call doubles price

                Console.SetCursorPosition(mapX + 1, 10);
                Console.Write(" Current market price: " + mainPlayer.shop.healthUpgradeCost);
            }
            else
                Console.SetCursorPosition(mapX + 1, 9);
                Console.Write(healthPickup);
                Console.Write(" Health Item Shop ");
                Console.SetCursorPosition(mapX + 1, 10);
                Console.Write(" Current market price: " + mainPlayer.shop.healthUpgradeCost);
        }
        public void UpdateDamageUIInfo(string type)
        {
            if (type == "Damage")
            {
                Console.SetCursorPosition(mapX + 1, 11);
                Console.Write(damagePickup);
                Console.Write(" Damage Item Shop ");

                mainPlayer.shop.UpdateUpgradeCosts(type); // every call doubles price

                Console.SetCursorPosition(mapX + 1, 12);
                Console.Write(" Current market price: " + mainPlayer.shop.damageUpgradeCost);
            }
            else
                Console.SetCursorPosition(mapX + 1, 11);
                Console.Write(damagePickup);
                Console.Write(" Damage Item Shop ");
                Console.SetCursorPosition(mapX + 1, 12);
                Console.Write(" Current market price: " + mainPlayer.shop.damageUpgradeCost);
        }
        public void ClearConsoleLine(int line)
        {
            // Set the cursor to the beginning of the specified line
            Console.SetCursorPosition(0, line);

            // Clear the line by writing spaces
            Console.Write(new string(' ', Console.WindowWidth));

            // Reset cursor position back to the start of the line (optional)
            Console.SetCursorPosition(0, line);
        }
        /// <summary>
        /// Draws HUD under game map
        /// </summary>
        public void DrawHUD() //Add to a UIManager Class
        {
            ClearConsoleLine(25);
            //Draws HUD.
            Console.SetCursorPosition(0,mapY + 1);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" Current Quest: ");
            questManager.UpdateActiveQuest();
            Console.WriteLine(questManager.activeQuest + "       ");
            SetColorDefault();
            string enemyHUDString = "{0} has Hp: {1} Armour: {2}     ";
            string FormatString = "HP: {0 } / {1 }  Damage: {2}  Armour: {3} Money: {4}   ";
            Console.WriteLine(string.Format(FormatString, mainPlayer.playerHealth, Settings.playerMaxHP , mainPlayer.playerDamage, mainPlayer.playerArmour, mainPlayer.shop.playerCoins));
            if (mainPlayer.enemyHitName == "")
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(string.Format(enemyHUDString, mainPlayer.enemyHitName, mainPlayer.enemyHitHealth, mainPlayer.enemyHitArmor));

            }
        }
    }
}