using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    /// <summary>
    /// The basic info needed for a plasmoid type enemy
    /// </summary>
    internal class Plasmoid : Enemy
    {
        //All info needed for plasmoid enemies. 
        public int BaseHP;
        public int BaseDamage;
        public Plasmoid(ConsoleColor color, Settings settings)
        {
            avatar = ((char)6);
            avatarColor = color;
            BaseHP = settings.PlasmoidBaseHP;
            BaseDamage = settings.PlasmoidBaseDamage;
        }
        public void SetEnemyStats()
        {
            healthSystem.health = 3 + levelNumber;
            healthSystem.armor = levelNumber - 1;
            enemyDamage = levelNumber;
        }
    }
}
