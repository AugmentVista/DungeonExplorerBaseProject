using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_OOP_
{
    internal class QuestManager
    {
        Player player;
        Map map;
        public static int questsCompleted = 0;

        public Queue<string> questsOrder;

        public string firstQuestDetails = " Make a purchase at a shop ";
        public string secondQuestDetails = " Acquire Wealth (20 or more money)";
        public string thirdQuestDetails = " Complete level 1";
        public string fourthQuestDetails = " Slay 10 enemies";
        public string fifthQuestDetails = " Complete level 2";
        public string sixthQuestDetails = " Complete final level";

        public string activeQuest;
        public void Start()
        {
            questsOrder = new Queue<string>();
            questsOrder.Enqueue(firstQuestDetails);
            questsOrder.Enqueue(secondQuestDetails);
            questsOrder.Enqueue(thirdQuestDetails);
            questsOrder.Enqueue(fourthQuestDetails);
            questsOrder.Enqueue(fifthQuestDetails);
            questsOrder.Enqueue(sixthQuestDetails);
            UpdateActiveQuest();
        }
        public void UpdateActiveQuest()
        {
            switch (questsCompleted)
            {
                case 0:
                    activeQuest = firstQuestDetails;
                    break;
                case 1:
                    activeQuest = secondQuestDetails;
                    break;
                case 2:
                    activeQuest = thirdQuestDetails;
                    break;
                case 3:
                    activeQuest = fourthQuestDetails;
                    break;
                case 4:
                    activeQuest = fifthQuestDetails;
                    break;
                case 5:
                    activeQuest= sixthQuestDetails;
                    break;
                case 6:
                    activeQuest = "You've completed all the quests";
                    break; 
                default: activeQuest = "No active quests";
                    break;
            }
        }

        public void QuestUI()
        {
            map.UpdateQuestUIInfo();
        }

        public void SetPlayer(Player player)
        { 
            this.player = player;
        }

        public void SetMap(Map map)
        {
            this.map = map;
        }

    }
}