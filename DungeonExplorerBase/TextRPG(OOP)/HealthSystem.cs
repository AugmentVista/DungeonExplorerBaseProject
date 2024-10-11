using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Health system used by all characters, handles damage and healing. 
    /// </summary>
    internal class HealthSystem
    {
        public int health;
        public int armour;
        public bool IsAlive;
        public HealthSystem() //Constructor
        {
            IsAlive = true;
            armour = 0;
        }
        /// <summary>
        /// heals player for HpGain value, max health needed for clamping
        /// </summary>
        /// <param name="HpGain"></param>
        /// <param name="maxHeath"></param>
        public void Heal(int HpGain, int maxHeath) //Health gain and health max needed to not over heal. 
        {
            health += HpGain;
            if(health > maxHeath)
            {
                health = maxHeath;
            }
        }
        /// <summary>
        /// Damages what ever is hit by passed in damage value
        /// </summary>
        /// <param name="Damage"></param>
        public void TakeDamage(int Damage) //Damage taking system.
        {
            if (Damage / armour <= 1) { Damage = 1; }
            health -= Damage / armour;
            if(health <= 0 )
            {
                health = 0;
                IsAlive = false;
            }
        }
        /// <summary>
        /// Sets max health for start of game
        /// </summary>
        /// <param name="maxHP"></param>
        public void SetHealth(int maxHP) //Sets HP for start of game.
        {
            health = maxHP;
        }
        /// <summary>
        /// Returns current HP, used to check if player is alive
        /// </summary>
        /// <returns></returns>
        public int GetHealth() //returns current HP.
        {
            return health;
        }
        /// <summary>
        /// Ups armor stat bt passed in value
        /// </summary>
        /// <param name="armorUp"></param>
        public void IncreaseArmour(int armorUp) //Increses Armor
        {
            armour += armorUp;
        }

        public int GetArmour() //returns current armour.
        {
            return armour;
        }
    }
}
