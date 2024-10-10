using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    internal class QuestManager
    {
        Player player;
        Map map;

        public List<string> questsOrder;

        public void UpdateActiveQuest()
        {
            if (questsOrder != null || questsOrder.Count > 0)
            {

            }
            else
            { 
                FirstQuest();
            }
        }

        public void SetPlayer(Player player)
        { 
            this.player = player;
        }
        public void SetMap(Map map)
        {
            this.map = map;
        }

        public void FirstQuest()
        {
            player.gameMap.UpdateQuestUIInfo();
        }

    }
}
