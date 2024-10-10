using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Program equivilent, runs the game. 
    /// </summary>
    internal class DungeonExplorer
    {
        static GameManager gameManager = new GameManager();
        static void Main()
        {
            //Plays game...
            gameManager.PlayGame();
        }
    }
}
