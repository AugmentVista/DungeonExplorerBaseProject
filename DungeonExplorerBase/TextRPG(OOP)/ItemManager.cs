using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Places and manage all items on each floor. 
    /// </summary>
    internal class ItemManager
    {
        public List<Item> items;
        public Map gameMap;
        public ItemManager()
        {
            items = new List<Item>();
        }
        /// <summary>
        /// Generates item based on type and places them in the x/y positions
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddItemToList(string type, int x, int y)
        {
            Item item = new Item(type,x,y,gameMap);
            items.Add(item);
        }
        /// <summary>
        /// Initilizastion, passes in map.
        /// </summary>
        /// <param name="map"></param>
        public void Start(Map map)
        {
            gameMap = map;
        }
        /// <summary>
        /// Clears item list for level change
        /// </summary>
        public void ClearItemList()
        {
            items.Clear();
        }
        /// <summary>
        /// Calls method to draw all items to the map
        /// </summary>
        public void Draw()
        {
            DrawItemsToMap();
        }
        /// <summary>
        /// Draws the items to the map passed in by the start function
        /// </summary>
        public void DrawItemsToMap()
        {
            for(int i = 0; i < items.Count(); i++)
            {
                if(items[i].isActive == true)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = items[i].color;
                    Console.SetCursorPosition(items[i].position.x, items[i].position.y);
                    Console.Write(items[i].item);
                }
            }
        }
        /// <summary>
        /// Updates items, checks if player is standing on an item
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player)
        {
            CheckItemPositions(player);
        }
        /// <summary>
        /// If player is on an item, updates stats based on pickups
        /// </summary>
        /// <param name="player"></param>
        public void CheckItemPositions(Player player)
        {
            for(int i = 0; i < items.Count(); i++)
            {
                if(items[i].position.x == player.position.x && items[i].position.y == player.position.y)
                {
                    // call to shopManager to check if player can afford item
                    if (items[i].itemType == "Damage Pickup") 
                    {
                        player.playerDamageUps += items[i].gainAmount;
                        items[i].isActive = false;    
                    }
                    // call to shopManager to check if player can afford item
                    if (items[i].itemType == "Health Pickup" && player.healthSystem.health != player.PlayerMaxHP)
                    {
                        player.healthSystem.health += items[i].gainAmount;
                        items[i].isActive = false;  
                    }
                    // call to shopManager to check if player can afford item
                    if (items[i].itemType == "Armor Pickup") 
                    {
                        player.healthSystem.armor += items[i].gainAmount;
                        items[i].isActive = false;  
                    }
                    // else write "Sorry, Link. I can't give credit. Come back when you're a little... mmmmm... richer!"
                }
            }
        }
    }
}
