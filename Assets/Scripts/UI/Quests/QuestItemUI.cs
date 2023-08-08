using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;
        
        Quest quest;
        public void Setup(Quest quest)
        {
            this.quest = quest;
            title.text = quest.Title;
            progress.text = "0/" + quest.GetObjectCount();
        }

        public Quest GetQuest()
        {
            return this.quest;
        }
        
    }

}