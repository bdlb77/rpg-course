using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectIncompletePrefab;
        public void Setup(QuestStatus status)
        {
            var quest = status.Quest;
            title.text = quest.Title;
            objectiveContainer.DetachChildren();

            foreach(var objective in quest.GetObjectives())
            {
                var usedPrefab = objectIncompletePrefab;
                if (status.IsObjectiveComplete(objective))
                {
                    usedPrefab = objectivePrefab;
                }
                 
                GameObject objectiveInstance = Instantiate(usedPrefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();


                objectiveText.text = objective;

            }
        }
    }

}