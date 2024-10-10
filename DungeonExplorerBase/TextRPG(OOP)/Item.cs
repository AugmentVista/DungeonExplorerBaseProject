using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Any and all types of pickups
    /// </summary>
    internal class Item
    {
        public string itemType;
        public int gainAmount;
        public bool isActive;
        public Position position;
        public Char item;
        public ConsoleColor color;
        public int index;
        public Item(string type, int x, int y, Map map)
        {
            isActive = true;
            itemType = type;
            if(itemType == "Health Pickup")
            {
                item = ((char)3);
                color = ConsoleColor.Red;
                gainAmount = map.levelNumber;
            }
            if(itemType == "Coin")
            {
                item = ((char)164);
                color = ConsoleColor.DarkYellow;
                gainAmount = 1;
            }
            if(itemType == "Armor Pickup")
            {
                item = ((char)21);
                color = ConsoleColor.DarkBlue;
                gainAmount = 1 * map.levelNumber;
            }
            position.x = x;
            position.y = y;
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
