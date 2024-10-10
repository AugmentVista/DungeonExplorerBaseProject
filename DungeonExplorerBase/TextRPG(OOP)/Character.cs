using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Base class for player and all enemies. holds position and health system.
    /// </summary>
    internal abstract class Character
    {
        public HealthSystem healthSystem;
        public Position position;
        public string name;
        public Character()
        {
            healthSystem = new HealthSystem();
            position.x = 0;
            position.y = 0;
        }
        public struct Position
        {
            public int x;
            public int y;
            public int maxX;
            public int maxY;
        }
    }
}
