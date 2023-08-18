using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        List<QuestStatus> statuses = new List<QuestStatus>();
        public event Action OnUpdate;

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            
            var newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            OnUpdate?.Invoke();
        }

        private bool HasQuest(Quest quest)
        {
            var questStatus = GetQuestStatus(quest);

            return questStatus != null;
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            var questStatus = GetQuestStatus(quest);
            questStatus.CompleteObjective(objective);
            OnUpdate?.Invoke();

        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.Quest == quest)
                {
                    return status;
                }
            }
            return null;

        }
    }

}