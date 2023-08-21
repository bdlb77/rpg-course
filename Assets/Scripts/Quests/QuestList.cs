using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable
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

        public void CompleteObjective(Quest quest, string objectiveRef)
        {
            var questStatus = GetQuestStatus(quest);
            questStatus.CompleteObjective(objectiveRef);
            if (questStatus.IsComplete())
            {
                GiveReward(quest);
            }
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
        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.GetRewards())
            {
                // questlist is already on player.
                var success = GetComponent<GameDevTV.Inventories.Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                if (!success)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            var stateList = state as List<object>;
            if (stateList == null) return;

            statuses.Clear();
            foreach (object objectState in stateList)
            {
               statuses.Add(new QuestStatus(objectState));
            }
        }
    }

}