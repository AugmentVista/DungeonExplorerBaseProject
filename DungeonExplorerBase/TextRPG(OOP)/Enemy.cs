using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Base class for all enemies. Handles all enemy movement.
    /// </summary>
    internal class Enemy : Character
    {
        //public HealthSystem EnemyHealth; old
        public int enemyDamage;
        public int enemyMaxHP;
        public int enemyNumber;
        public int enemyMaxX;
        public int enemyMaxY;
        public string enemyType;
        public int levelNumber;
        public Char avatar;
        public ConsoleColor avatarColor;
        public Random moveRoll;
        public Enemy()
        {
            enemyMaxHP = 2;
            enemyDamage = 1;
            healthSystem.SetHealth(enemyMaxHP);
            enemyType = "Slime";
            name = enemyType;
            avatar = ((char)127);
            moveRoll = new Random();
            //Console.Write("Initialized enemy");
        }
        /// <summary>
        /// Used to keep enemy in map
        /// </summary>
        /// <param name="map"></param>
        public void SetEnemyMaxPosition(Map map)
        {
            int mapX;
            int mapY;
            mapX = map.activeMap.GetLength(1);
            mapY = map.activeMap.GetLength(0);
            enemyMaxX = mapX - 1;
            enemyMaxY = mapY - 1;
        }
        /// <summary>
        /// Handels enemy movement and interactions based on type, map needed for collisions.
        /// </summary>
        /// <param name="gameMap"></param>
        public void MoveEnemy(Map gameMap)
        {
            int enemyMoveX;
            int enemyMoveY;
            // move up
            if(healthSystem.IsAlive == false)
            {
                position.x = 0;
                position.y = 0;
            }
            if(enemyType == "Plasmoid") // this type moves at random
            {
                int moveResult = moveRoll.Next(1,5);
                Debug.WriteLine("roll result = " + moveResult);
                if(moveResult == 1)
                {
                    enemyMoveY = position.y - 1;
                    if(enemyMoveY <= 0)
                    {
                        enemyMoveY = 0;
                    }
                    if(gameMap.CretureInTarget(enemyMoveY, position.x) && gameMap.index != enemyNumber) // != enemyNumber is needed to prevent enemies from self harming
                    {
                        gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                        Debug.WriteLine("hit " + gameMap.characters[gameMap.index].name);
                        enemyMoveY = position.y;
                        position.y = enemyMoveY;
                        return;
                    }
                    if(gameMap.IsPlayerInTarget(enemyMoveY, position.x))
                    {
                        gameMap.characters[0].healthSystem.TakeDamage(enemyDamage);
                        return;
                    }
                    if(gameMap.CheckTile(enemyMoveY, position.x) == false)
                    {
                        Debug.WriteLine("HitWall");
                        enemyMoveY = position.y;
                        position.y = enemyMoveY;
                        return;
                    }
                    else
                    {
                        //Debug.WriteLine("Moved up");
                        position.y = enemyMoveY;
                        if(position.y <= 0)
                        {
                            position.y = 0;
                        }
                    }
                }
                // move down
                if(moveResult == 2)
                {
                    enemyMoveY = position.y + 1;
                    if(enemyMoveY >= enemyMaxY)
                    {
                        enemyMoveY = enemyMaxY;
                    }
                    if(gameMap.CretureInTarget(enemyMoveY, position.x) && gameMap.index != enemyNumber)
                    {
                        gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                        Debug.WriteLine("hit " + gameMap.characters[gameMap.index].name);
                        enemyMoveY = position.y;
                        position.y = enemyMoveY;
                        return;
                    }
                    if(gameMap.IsPlayerInTarget(enemyMoveY, position.x))
                    {
                        gameMap.characters[0].healthSystem.TakeDamage(enemyDamage);
                        return;
                    }
                    if(gameMap.CheckTile(enemyMoveY, position.x) == false)
                    {
                        Debug.WriteLine("HitWall");
                        enemyMoveY = position.y;
                        position.y = enemyMoveY;
                        return;
                    }
                    else
                    {
                        Debug.WriteLine("Moved down");
                        position.y = enemyMoveY;
                    if(position.y >= enemyMaxY)
                    {
                        position.y = enemyMaxY;
                    }
                }
            }
            // move left
            if(moveResult == 3)
            {
                enemyMoveX = position.x - 1;
                if(enemyMoveX >= enemyMaxX)
                {
                    enemyMoveX = enemyMaxX;
                }
                if(enemyMoveX <= 0)
                {
                    enemyMoveX = 0;
                }
                if(gameMap.CretureInTarget(position.y, enemyMoveX)&& gameMap.index-1 != enemyNumber)
                {
                    gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                    Debug.WriteLine("hit " + gameMap.characters[gameMap.index].name);
                    enemyMoveX = position.x;
                    position.x = enemyMoveX;
                    return;
                }
                if(gameMap.IsPlayerInTarget(position.y, enemyMoveX))
                {
                    gameMap.characters[0].healthSystem.TakeDamage(enemyDamage);
                    return;
                }
                if(gameMap.CheckTile(position.y, enemyMoveX) == false)
                {
                    Debug.WriteLine("HitWall");
                    enemyMoveX = position.x;
                    position.x = enemyMoveX;
                    return;
                }
                else
                {
                    //Debug.WriteLine("Moved left");
                    position.x = enemyMoveX;
                    if(position.x <= 0)
                    {
                        position.x = 0;
                    }
                }
            }
            // move
            if(moveResult == 4)
            {
                enemyMoveX = position.x + 1;
                if(gameMap.CretureInTarget(position.y, enemyMoveX)&& gameMap.index != enemyNumber)
                {
                    gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                    Debug.WriteLine("hit " + gameMap.characters[gameMap.index].name);
                    enemyMoveX = position.x;
                    position.x = enemyMoveX;
                    return;
                }
                if(gameMap.IsPlayerInTarget(position.y, enemyMoveX))
                {
                    gameMap.characters[0].healthSystem.TakeDamage(enemyDamage);
                    return;
                }
                if(gameMap.CheckTile(position.y, enemyMoveX) == false)
                {
                    Debug.WriteLine("HitWall");
                    enemyMoveX = position.x;
                    position.x = enemyMoveX;
                    return;
                }
                else
                {
                    //Debug.WriteLine("Moved right");
                    position.x = enemyMoveX;
                    if(position.x >= enemyMaxX)
                    {
                        position.x = enemyMaxX;
                    }
                }
            }
            }
            if(enemyType == "GoblinFolk") // this type will flee from player
            {
                int rangeMaxX = 7;
            int rangeMaxY = 5;
            int rangeX = position.x - gameMap.characters[0].position.x; //characters[0] is always the player!
            int rangeY = position.y - gameMap.characters[0].position.y;
            if((rangeX < rangeMaxX && rangeX > -rangeMaxX)&&(rangeY < rangeMaxY && rangeY > -rangeMaxY))
            {
                if(rangeX < rangeMaxX && rangeX > 0)
                {
                    enemyMoveX = position.x + 1;
                    Debug.WriteLine("Moved ");
                    if(gameMap.CretureInTarget(position.y, enemyMoveX)&& gameMap.index != enemyNumber)
                    {
                        gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                        enemyMoveX = position.x;
                        position.x = enemyMoveX;
                        return;
                    }
                    if(gameMap.CheckTile(position.y, enemyMoveX) == false)
                    {
                        Debug.WriteLine("HitWall");
                        enemyMoveX = position.x;
                        position.x = enemyMoveX;
                        return;
                    }
                    else
                    {
                        position.x = enemyMoveX;
                        if(gameMap.characters[0].position.x >= enemyMaxX)
                        {
                            position.x = enemyMaxX;
                        }
                        return;
                    }
                }
                if(rangeX > -rangeMaxX && rangeX < 0)
                {
                    enemyMoveX = position.x - 1;
                    if(enemyMoveX >= enemyMaxX)
                    {
                        enemyMoveX = enemyMaxX;
                    }
                    if(enemyMoveX <= 0)
                    {
                        enemyMoveX = 0;
                    }
                    if(gameMap.CretureInTarget(position.y, enemyMoveX)&& gameMap.index != enemyNumber)
                    {
                        gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                        enemyMoveX = position.x;
                        position.x = enemyMoveX;
                        return;
                    }
                    if(gameMap.CheckTile(position.y, enemyMoveX) == false)
                    {
                        Debug.WriteLine("HitWall");
                        enemyMoveX = position.x;
                        position.x = enemyMoveX;
                        return;
                    }
                    else
                    {
                        position.x = enemyMoveX;
                        if(gameMap.characters[enemyNumber].position.x >= enemyMaxX)
                        {
                            position.x = enemyMaxX;
                        }
                        return;
                    }
                }
            }
            if((rangeX < rangeMaxX && rangeX > -rangeMaxX)&&(rangeY < rangeMaxY && rangeY > -rangeMaxY))
            {
                if(rangeY < rangeMaxY && rangeY > 0)
                {
                    enemyMoveY = position.y + 1;
                    if(enemyMoveY >= enemyMaxY)
                    {
                        enemyMoveY = enemyMaxY;
                    }
                    if(gameMap.CretureInTarget(enemyMoveY, position.x) && gameMap.index != enemyNumber) // != enemyNumber is needed to prevent enemies from self harming
                    {
                        gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                        enemyMoveY = position.y;
                        position.y = enemyMoveY;
                        return;
                    }
                    if(gameMap.CheckTile(enemyMoveY, position.x) == false)
                    {
                        Debug.WriteLine("HitWall");
                        enemyMoveY = position.y;
                        position.y = enemyMoveY;
                        return;
                    }
                    else
                    {
                        position.y = enemyMoveY;
                        if(position.y >= enemyMaxY)
                        {
                            position.y = enemyMaxY;
                        }
                        return;
                    }
                }
                if(rangeY > -rangeMaxY && rangeY < 0)
                {
                    enemyMoveY = position.y - 1;
                    if(enemyMoveY <= 0)
                    {
                        enemyMoveY = 0;
                    }
                    if(gameMap.CretureInTarget(enemyMoveY, position.x) && gameMap.index != enemyNumber) // != enemyNumber is needed to prevent enemies from self harming
                    {
                        gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                        enemyMoveY = position.y;
                        position.y = enemyMoveY;
                        return;
                    }
                    if(gameMap.CheckTile(enemyMoveY, position.x) == false)
                    {
                        Debug.WriteLine("HitWall");
                        enemyMoveY = position.y;
                        position.y = enemyMoveY;
                        return;
                    }
                    else
                    {
                        position.y = enemyMoveY;
                        if(position.y <= 0)
                        {
                            position.y = 0;
                        }
                        return;
                    }
                }
                else
                {return;}
            }
            }
            if(enemyType == "Construct") // this type will chase player
            {
                int rangeMaxX = 7;
                int rangeMaxY = 7;
                int rangeX = position.x - gameMap.characters[0].position.x; //characters[0] is always the player!
                int rangeY = position.y - gameMap.characters[0].position.y;
                if((rangeX < rangeMaxX && rangeX > -rangeMaxX)&&(rangeY < rangeMaxY && rangeY > -rangeMaxY))
                {
                    if(rangeX < rangeMaxX && rangeX > 0)
                    {
                        enemyMoveX = position.x - 1;
                        Debug.WriteLine("Moved ");
                        if(gameMap.CretureInTarget(position.y, enemyMoveX)&& gameMap.index != enemyNumber)
                        {
                            //gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                            enemyMoveX = position.x;
                            position.x = enemyMoveX;
                            return;
                        }
                        if(gameMap.IsPlayerInTarget(position.y, enemyMoveX))
                        {
                            gameMap.characters[0].healthSystem.TakeDamage(enemyDamage);
                            return;
                        }
                        if(gameMap.CheckTile(position.y, enemyMoveX) == false)
                        {
                            Debug.WriteLine("HitWall");
                            enemyMoveX = position.x;
                            position.x = enemyMoveX;
                            return;
                        }
                        else
                        {
                            position.x = enemyMoveX;
                            if(gameMap.characters[0].position.x >= enemyMaxX)
                            {
                                position.x = enemyMaxX;
                            }
                            return;
                        }
                    }
                    if(rangeX > -rangeMaxX && rangeX < 0)
                    {
                        enemyMoveX = position.x + 1;
                        if(enemyMoveX >= enemyMaxX)
                        {
                            enemyMoveX = enemyMaxX;
                        }
                        if(enemyMoveX <= 0)
                        {
                            enemyMoveX = 0;
                        }
                        if(gameMap.CretureInTarget(position.y, enemyMoveX)&& gameMap.index != enemyNumber)
                        {
                            //gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                            enemyMoveX = position.x;
                            position.x = enemyMoveX;
                            return;
                        }
                        if(gameMap.IsPlayerInTarget(position.y, enemyMoveX))
                        {
                            gameMap.characters[0].healthSystem.TakeDamage(enemyDamage);
                            return;
                        }
                        if(gameMap.CheckTile(position.y, enemyMoveX) == false)
                        {
                            Debug.WriteLine("HitWall");
                            enemyMoveX = position.x;
                            position.x = enemyMoveX;
                            return;
                        }
                        else
                        {
                            position.x = enemyMoveX;
                            if(gameMap.characters[enemyNumber].position.x >= enemyMaxX)
                            {
                                position.x = enemyMaxX;
                            }
                            return;
                        }
                    }
                }
                if((rangeY < rangeMaxY && rangeY > -rangeMaxY)&&(rangeX < rangeMaxX && rangeX > -rangeMaxX))
                {
                    if(rangeY < rangeMaxY && rangeY > 0)
                    {
                        enemyMoveY = position.y - 1;
                        if(enemyMoveY >= enemyMaxY)
                        {
                            enemyMoveY = enemyMaxY;
                        }
                        if(gameMap.CretureInTarget(enemyMoveY, position.x) && gameMap.index != enemyNumber) // != enemyNumber is needed to prevent enemies from self harming
                        {
                            //gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                            enemyMoveY = position.y;
                            position.y = enemyMoveY;
                            return;
                        }
                        if(gameMap.IsPlayerInTarget(enemyMoveY, position.x))
                        {
                            gameMap.characters[0].healthSystem.TakeDamage(enemyDamage);
                            return;
                        }
                        if(gameMap.CheckTile(enemyMoveY, position.x) == false)
                        {
                            Debug.WriteLine("HitWall");
                            enemyMoveY = position.y;
                            position.y = enemyMoveY;
                            return;
                        }
                        else
                        {
                            position.y = enemyMoveY;
                            if(position.y >= enemyMaxY)
                            {
                                position.y = enemyMaxY;
                            }
                            return;
                        }
                    }
                    if(rangeY > -rangeMaxY && rangeY < 0)
                    {
                        enemyMoveY = position.y + 1;
                        if(enemyMoveY <= 0)
                        {
                            enemyMoveY = 0;
                        }
                        if(gameMap.CretureInTarget(enemyMoveY, position.x) && gameMap.index != enemyNumber) // != enemyNumber is needed to prevent enemies from self harming
                        {
                            //gameMap.characters[gameMap.index].healthSystem.TakeDamage(enemyDamage);
                            enemyMoveY = position.y;
                            position.y = enemyMoveY;
                            return;
                        }
                        if(gameMap.IsPlayerInTarget(enemyMoveY, position.x))
                        {
                            gameMap.characters[0].healthSystem.TakeDamage(enemyDamage);
                            return;
                        }
                        if(gameMap.CheckTile(enemyMoveY, position.x) == false)
                        {
                            Debug.WriteLine("HitWall");
                            enemyMoveY = position.y;
                            position.y = enemyMoveY;
                            return;
                        }
                        else
                        {
                            position.y = enemyMoveY;
                            if(position.y <= 0)
                            {
                                position.y = 0;
                            }
                            return;
                        }
                    }
                    else
                    {return;}
                }
            }
        }
        /// <summary>
        /// Sets enemy level to the floor number, used for scaling enemy difculty.
        /// </summary>
        /// <param name="level"></param>
        public void SetLevelNumber(int level)
        {
            levelNumber = level;
        }
    }
}
