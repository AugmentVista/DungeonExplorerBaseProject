using System;
using System.Threading;
using System.Diagnostics;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Runs and manages game functions. Most method calls live here.
    /// </summary>
    internal class GameManager
    {
        private Player mainPlayer;
        private EnemyManager enemyManager;
        public Map gameMap;
        public ItemManager itemManager;
        public Settings settings;
        public ShopManager shop;
        public QuestManager questManager;
        public static bool hasSaveFile = false;

        public void PlayGame()
        {
            Debug.WriteLine("Starting Game");
            StartUp();
            Intro();
            QuestManager.questsCompleted = 0;
            Console.WriteLine("Hit L to load your last save file");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Hit N to begin a New game.");
            Console.WriteLine();
            Console.WriteLine();
            CheckForSaveState();
        }

        private void StartUp()
        {
            Console.CursorVisible = false;
            settings = new Settings();
            itemManager = new ItemManager();
            shop = new ShopManager();
            questManager = new QuestManager();
            gameMap = new Map(itemManager, questManager);
            enemyManager = new EnemyManager(gameMap, settings, shop);
            mainPlayer = new Player(gameMap,itemManager, shop, questManager);
        }

        private void Intro()
        {
            Console.SetWindowSize(120, 40);
            Debug.WriteLine("Into!");
            Console.WriteLine("Welcome to Dungeon Explorer!");
            Console.WriteLine();
            Console.Write("Beat up the inhabitants of this dungeon and climb to the 3rd floor to yoink this thing ");
            gameMap.DrawFinalLoot();
            Console.WriteLine();
            Console.Write("Shop here ");
            gameMap.DrawDamageUpgrade();
            Console.Write(" to upgrade your damage");
            Console.WriteLine();
            Console.Write("Shop here ");
            gameMap.DrawHealthPickup();
            Console.WriteLine(" to upgprade your health ");
            Console.Write("Shop here ");
            gameMap.DrawArmor();
            Console.Write(" to upgrade your armour ");
            Console.WriteLine();
            Console.Write("Commit assault against the creatures of this dungeon for cash");
            Console.WriteLine();
            Console.WriteLine("Spend that cash to improve yourself");
            Console.ReadKey(true);
            Console.Clear();
        }

        public void CheckForSaveState()
        {
            ConsoleKeyInfo playerInput = Console.ReadKey(true);

            if (playerInput.Key == ConsoleKey.L)
            {
                try
                {
                    Settings.LoadSettings();
                    Settings.LoadStaticSettings();
                    Player.LoadPlayer();
                    ShopManager.LoadShop();
                    hasSaveFile = true;
                    Debug.WriteLine(hasSaveFile);
                    SetUpGame();
                    DungeonGameLoop();
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    Console.Clear();
                    Console.WriteLine("Save file not found: " + ex.Message);
                    hasSaveFile = false;
                    StartNewGame();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    Console.Clear();
                    Console.WriteLine("Error loading game data: " + ex.Message);
                    hasSaveFile = false;
                    StartNewGame();
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                    hasSaveFile = false;
                    StartNewGame();
                }
            }
            else if (playerInput.Key == ConsoleKey.N)
            {
                SetUpGame();
                //Settings.SaveStaticSettings(settings);
                //settings.SaveSettings();
                //mainPlayer.SavePlayer();
                //shop.SaveShop();
                hasSaveFile = false;
                DungeonGameLoop();
            }
            else if (playerInput.Key != ConsoleKey.L && playerInput.Key != ConsoleKey.N)
            {
                StartNewGame();
            }
        }

        private void SetUpGame()
        {
            shop.Start();
            questManager.Start();
            itemManager.Start(gameMap);
            gameMap.Start(mainPlayer, enemyManager);
            mainPlayer.Start();
            gameMap.Draw();
            itemManager.Draw();
            mainPlayer.Draw();
            enemyManager.Draw();
        }

        private void StartNewGame()
        {
            Console.WriteLine("No save file found on record, beginning new game");
            Thread.Sleep(2000);
            SetUpGame();
            Settings.SaveStaticSettings(settings);
            settings.SaveSettings();
            mainPlayer.SavePlayer();
            shop.SaveShop();
            hasSaveFile = true;
            DungeonGameLoop();
        }

        private void DungeonGameLoop()
        {
            Debug.WriteLine("Running GameLoop");
            while (mainPlayer.gameIsOver != true && mainPlayer.gameWon != true)
            {
                Console.CursorVisible = false;
                CheckPlayerCondition();
                gameMap.Update();
                mainPlayer.Update();
                gameMap.Draw();
                mainPlayer.Draw();
                itemManager.Update(mainPlayer);
                itemManager.Draw();
                enemyManager.Update();
                enemyManager.Draw();
                mainPlayer.SavePlayer();
                shop.SaveShop();
            }
            EndGame();
        }

        private void CheckPlayerCondition()
        {
            if(mainPlayer.healthSystem.IsAlive == false)
            {
                mainPlayer.gameIsOver = true;
            }
            CheckPauseState(ShopManager.Paused);
        }

        public static void CheckPauseState(bool isPaused)
        {
            while (isPaused)
            {
                // do nothing

                if (!isPaused)
                {
                    continue;
                }
            }
        }

        private void EndGame()
        {
            string FormatString = "You had {0} damage, {1} armor, and {2} HP remaining!";
            Debug.WriteLine("Ending Game");
            if (mainPlayer.gameIsOver && mainPlayer.gameWon == true)
            {
                Debug.WriteLine("Player won");
                Console.Clear();
                Console.WriteLine("You Won!, I didn't think you'd do it, well done ");
                Console.WriteLine("You finished " + QuestManager.questsCompleted.ToString() + " Quests!");
                Console.WriteLine();
                Console.WriteLine(string.Format(FormatString, mainPlayer.playerDamageUps, mainPlayer.healthSystem.armour, mainPlayer.healthSystem.health));
                Console.WriteLine();
                Console.WriteLine("Congratulations");
                Console.ReadKey(true);
                Thread.Sleep(3000);
                Environment.Exit(0);
            }
            if (mainPlayer.gameIsOver && mainPlayer.gameWon != true)
            {
                Debug.WriteLine("Player lost");
                Console.WriteLine("You finished " + QuestManager.questsCompleted.ToString() + " Quests!");
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine("You have lost, that's okay. Try again?");
                Thread.Sleep(3000);
                PlayGame();
            }
        }
    }
}
