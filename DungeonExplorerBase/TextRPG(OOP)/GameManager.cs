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
        public ShopManager shop;
        public QuestManager questManager;
        /// <summary>
        /// Gets all references so game is ready to start up
        /// </summary>
        private void StartUp()
        {
            Console.CursorVisible = false;
            Debug.WriteLine("Setting Up characters");
            settings = new Settings();
            itemManager = new ItemManager();
            shop = new ShopManager();
            questManager = new QuestManager();
            gameMap = new Map(itemManager);
            enemyManager = new EnemyManager(gameMap, settings, shop);
            mainPlayer = new Player(gameMap,itemManager, settings, shop, questManager);
        } 
        /// <summary>
        /// Calls Start methods for all things needed in the game.
        /// </summary>
        private void SetUpGame()
        {
            Debug.WriteLine("Setting up starting map");
            itemManager.Start(gameMap);
            gameMap.Start(mainPlayer, enemyManager, questManager);
            mainPlayer.Start();
            shop.Start();
            gameMap.Draw();
            itemManager.Draw();
            mainPlayer.Draw();
            enemyManager.Draw();
            questManager.UpdateActiveQuest();
        }
        /// <summary>
        /// Handels game ending, for both win and loss.
        /// </summary>
        private void EndGame()
        {
            string FormatString = "You had {0} damage, {1} armor, and {2} HP remaining!";
            Debug.WriteLine("Ending Game");
            if(mainPlayer.gameIsOver && mainPlayer.gameWon == true)
            {
                Debug.WriteLine("Player won");
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine("You Won!, I didn't think you'd do it, well done ");
                Console.WriteLine();
                Console.WriteLine(string.Format(FormatString,mainPlayer.playerDamageUps,mainPlayer.healthSystem.armor,mainPlayer.healthSystem.health));
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
                Console.WriteLine("You have lost, that's okay. Try again?");
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
                shop.Update();
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
            Console.SetWindowSize(120,30);
            Debug.WriteLine("Into!");
            Console.WriteLine("Welcome to Dungeon Explorer!"); // placeholderTitle
            Console.WriteLine();
            Console.Write("Beat up the inhabitants of this dungeon and climb to the 3rd floor to yoink this thing ");
            gameMap.DrawFinalLoot();
            Console.WriteLine();
            Console.Write("Purchase ");
            gameMap.DrawDamageUpgrade();
            Console.Write(" to upgrade your damage");
            Console.WriteLine();
            Console.Write("Purchase ");
            gameMap.DrawHealthPickup();
            Console.WriteLine(" to upgprade your health ");
            Console.Write("Purchase "); 
            gameMap.DrawArmor();
            Console.Write(" to upgrade your armour ");
            Console.WriteLine();
            Console.Write("Commit assault against the creatures of this dungeon for cash");
            Console.WriteLine(" Use that cash to improve yourself");
            Console.WriteLine();
            Console.WriteLine("Press any key to get started!");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
