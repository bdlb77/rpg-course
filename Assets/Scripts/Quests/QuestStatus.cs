using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<string> completedObjectives = new List<string>();

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;
        }
        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
           QuestStatusRecord state = objectState as QuestStatusRecord;
           quest = Quest.GetByName(state.questName);
           completedObjectives = state.completedObjectives;
        }

        public Quest Quest { get { return quest; } }

        public int GetCompletedCount()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return completedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if (!quest.HasObjective(objective)) return;
            completedObjectives.Add(objective);
        }

        public object CaptureState()
        {
            var state = new QuestStatusRecord {
                questName = quest.name,
                completedObjectives = completedObjectives
            };
            return state;
        }

    }

}