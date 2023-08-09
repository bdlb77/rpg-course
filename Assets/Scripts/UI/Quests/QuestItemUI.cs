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
        
        QuestStatus status;
        public void Setup(QuestStatus status)
        {
            this.status = status;
            title.text = status.Quest.Title;
            progress.text = status.GetCompletedCount() + "/" + status.Quest.GetObjectiveCount();
        }

        public QuestStatus GetQuestStatus()
        {
            return this.status;
        }
        
    }

}