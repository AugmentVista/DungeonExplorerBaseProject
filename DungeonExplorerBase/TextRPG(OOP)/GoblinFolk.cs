using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    /// <summary>
    /// The basic info needed for a goblin folk type enemy
    /// </summary>
    internal class GoblinFolk : Enemy
    {
        public int BaseHP;
        public int BaseDamage;
        public GoblinFolk(ConsoleColor color, Settings settings)
        {
            avatar = ((char)5);
            avatarColor = color;
            BaseHP = settings.GoblinFolkBaseHP;
            BaseDamage = settings.GoblinFolkBaseDamage;
        }
        public void SetEnemyStats()
        {
            healthSystem.health = BaseHP + levelNumber;
            healthSystem.armor = 0;
            enemyDamage = BaseDamage;
        }
    }
}
