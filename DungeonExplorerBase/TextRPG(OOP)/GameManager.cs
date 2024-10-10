using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <summary>
        /// Gets all references so game is ready to start up
        /// </summary>
        private void StartUp()
        {
            Console.CursorVisible = false;
            Debug.WriteLine("Setting Up characters");
            settings = new Settings();
            itemManager = new ItemManager();
            gameMap = new Map(itemManager);
            enemyManager = new EnemyManager(gameMap, settings);
            mainPlayer = new Player(gameMap,itemManager, settings);
        } 
        /// <summary>
        /// Calls Start methods for all things needed in the game.
        /// </summary>
        private void SetUpGame()
        {
            Debug.WriteLine("Setting up starting map");
            itemManager.Start(gameMap);
            gameMap.Start(mainPlayer, enemyManager);
            mainPlayer.Start();
            gameMap.Draw();
            itemManager.Draw();
            mainPlayer.Draw();
            enemyManager.Draw();
        }
        /// <summary>
        /// Handels game ending, for both win and loss.
        /// </summary>
        private void EndGame()
        {
            string FormatString = "You had {0} coins, {1} armor, and {2} HP remaining!";
            Debug.WriteLine("EndingGame");
            if(mainPlayer.gameIsOver && mainPlayer.gameWon == true)
            {
                Debug.WriteLine("Player won");
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine("You Won!");
                Console.WriteLine();
                Console.WriteLine(string.Format(FormatString,mainPlayer.playerCoins,mainPlayer.healthSystem.armor,mainPlayer.healthSystem.health));
                Console.WriteLine();
                Console.WriteLine("Congratulations");
                Thread.Sleep(3000);
                Environment.Exit(0);
            }
            if(mainPlayer.gameIsOver && mainPlayer.gameWon != true)
            {
                Debug.WriteLine("Player lost");
                Thread.Sleep(2000); 
                Console.Clear();
                Console.WriteLine("You have lost. Restarting game!");
                Thread.Sleep(3000);
                PlayGame();
            }
        }
        /// <summary>
        /// Primary loop that gameplay takes place in. Calls all updates and Draws.
        /// </summary>
        private void DungeonGameLoop()
        {
            Debug.WriteLine("Running GameLoop");
            while(mainPlayer.gameIsOver != true && mainPlayer.gameWon != true)
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
            }
            EndGame();
        }
        /// <summary>
        /// Is the way to start the game
        /// </summary>
        public void PlayGame()
        {
            Debug.WriteLine("Starting Game");
            StartUp();
            Intro();
            SetUpGame();
            DungeonGameLoop();
        }
        /// <summary>
        /// Checks if player is dead
        /// </summary>
        private void CheckPlayerCondition()
        {
            Debug.WriteLine("Checking player");
            if(mainPlayer.healthSystem.IsAlive == false)
            {
                mainPlayer.gameIsOver = true;
            }
        }
        /// <summary>
        /// Runs game intro
        /// </summary>
        void Intro()
        {
            Debug.WriteLine("Into!");
            Console.WriteLine("Welcome to Dungeon Explorer!"); // placeholderTitle
            Console.WriteLine();
            Console.Write("Escape the dungeon and climb to the 2nd floor to find the chalace. ");
            gameMap.DrawFinalLoot();
            Console.WriteLine();
            Console.Write("Collect coins ");
            gameMap.DrawCoin();
            Console.Write(" to increase your attack power.");
            Console.WriteLine();
            Console.Write("Collect hearts to heal.");
            gameMap.DrawHealthPickup();
            Console.WriteLine();
            Console.Write("Collect peices of armor "); 
            gameMap.DrawArmor();
            Console.Write(" to up your defence.");
            Console.WriteLine();
            Console.Write("Avoid or fight the monsters!");
            Console.WriteLine();
            Console.WriteLine("Press any key to get started!");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
