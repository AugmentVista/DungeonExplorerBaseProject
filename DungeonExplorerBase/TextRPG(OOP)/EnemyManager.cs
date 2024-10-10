using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Handels and manages all enemies on each floor. 
    /// </summary>
    internal class EnemyManager
    {
        public List<Enemy> enemiesList; 
        public Map gameMap;
        public bool isFirstKobald;
        public Settings enemySettings;
        public EnemyManager(Map map, Settings settings)
        {
            isFirstKobald = true;
            gameMap = map;
            enemiesList = new List<Enemy>();
            enemySettings = settings;
        }
        /// <summary>
        /// Generates enemmy based on paramater type, and level number. level number used to determain enemy stats
        /// </summary>
        /// <param name="type"></param>
        /// <param name="levelNumber"></param>
        public void AddEnemiesToList(string type, int levelNumber)
        {
            if(type == "Plasmoid")
            {
                Plasmoid plasmoid = new Plasmoid(RandomConsoleColor(),enemySettings);
                plasmoid.enemyType = type;
                plasmoid.name = "Slime " + EnemyTypeCount(type);
                plasmoid.SetLevelNumber(levelNumber);
                plasmoid.SetEnemyMaxPosition(gameMap);
                plasmoid.SetEnemyStats();
                enemiesList.Add(plasmoid);
            }
            if(type == "Construct")
            {
                Construct construct = new Construct(RandomConsoleColor(),enemySettings);
                construct.enemyType = type;
                construct.name = "Living Armor " + EnemyTypeCount(type);
                construct.SetLevelNumber(levelNumber);
                construct.SetEnemyMaxPosition(gameMap);
                construct.SetEnemyStats();
                enemiesList.Add(construct);
            }
            if(type == "GoblinFolk")
            {
                GoblinFolk goblinFolk = new GoblinFolk(RandomConsoleColor(),enemySettings);
                goblinFolk.enemyType = type;
                if(isFirstKobald)
                {
                    goblinFolk.name = "Jim the Coward";
                    isFirstKobald = false;
                }
                else
                {
                    goblinFolk.name = "Goblin " + EnemyTypeCount(type);
                }
                goblinFolk.SetLevelNumber(levelNumber);
                goblinFolk.SetEnemyMaxPosition(gameMap);
                goblinFolk.SetEnemyStats();
                enemiesList.Add(goblinFolk);
            }
        }
        /// <summary>
        /// Goes through list of enemies and calls move function 
        /// </summary>
        public void Update()
        {
            for(int i = 0; i < enemiesList.Count(); i++)
            {
                Debug.WriteLine("Moving " + enemiesList[i].name);
                enemiesList[i].MoveEnemy(gameMap);
            }
        }
        /// <summary>
        /// Calls the drawn enemys to map function, passing in the Enemy list from enemy manager
        /// </summary>
        public void Draw()
        {
            gameMap.DrawEnemiesToMap(enemiesList);
        }
        /// <summary>
        /// Selects random color for each enemy when generated
        /// </summary>
        /// <returns></returns>
        public ConsoleColor RandomConsoleColor()
        {
            Random colorRoll = new Random();
            int rollResult = colorRoll.Next(1,7);
            ConsoleColor RandomColor = new ConsoleColor();
            if(rollResult == 1)
            {
                RandomColor = ConsoleColor.DarkBlue;
            }
            if(rollResult == 2)
            {
                RandomColor = ConsoleColor.DarkGreen;
            }
            if(rollResult == 3)
            {
                RandomColor = ConsoleColor.DarkMagenta;
            }
            if(rollResult == 4)
            {
                RandomColor = ConsoleColor.DarkRed;
            }
            if(rollResult == 5)
            {
                RandomColor = ConsoleColor.DarkYellow;
            }
            if(rollResult == 6)
            {
                RandomColor = ConsoleColor.Green;
            }
            return RandomColor;
        }
        /// <summary>
        /// Counts number of enemies of desired type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        int EnemyTypeCount(string type)
        {
            int enemyCount = 0;
            for(int i = 0; i < enemiesList.Count(); i++)
            {
                if(enemiesList[i].enemyType == type)
                {
                    enemyCount += 1;
                }
            }
            if(type == "GoblinFolk")
            {
                enemyCount -= 1;
            }
            return enemyCount;
        }
    }
}
