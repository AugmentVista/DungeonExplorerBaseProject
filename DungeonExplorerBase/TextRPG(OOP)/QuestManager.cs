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

        public List<string> questsOrder; // change to queue

        public string firstQuestDetails;
        public string secondQuestDetails;

        public void Start()
        {
            firstQuestDetails = " Earn your first dollar ";
            secondQuestDetails = "";
        }
        public void UpdateActiveQuest()
        {
            FirstQuest();

            // if firstquest completed 
            // switch (newquest)
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
