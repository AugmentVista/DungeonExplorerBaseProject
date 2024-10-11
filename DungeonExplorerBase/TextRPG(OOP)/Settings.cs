using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Holds the base values for all Characater
    /// </summary>
    internal class Settings
    {
        //Base values for all stats. 
        public int PlasmoidBaseHP = 3;
        public int PlasmoidBaseDamage = 0;
        public int ConstructBaseHP = 3;
        public int ConstructBaseDamage = 0;
        public int GoblinFolkBaseHP = 3;
        public int GoblinFolkBaseDamage = 0;



        /// <summary>
        /// Player stats are static as there is only ever one player
        /// </summary>
        public static int playerMaxHP = 25;
        public static int playerMaxArmour = 5;
        public static int playerMaxDamage = 10;
    }
}
