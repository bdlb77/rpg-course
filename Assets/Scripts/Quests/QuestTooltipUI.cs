using System;
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
        [SerializeField] TextMeshProUGUI rewardText;
        public void Setup(QuestStatus status)
        {
            var quest = status.Quest;
            title.text = quest.Title;
            
            foreach (Transform item in objectiveContainer)
            {
                Destroy(item.gameObject);
            }

            foreach (var objective in quest.GetObjectives())
            {
                var usedPrefab = objectIncompletePrefab;
                if (status.IsObjectiveComplete(objective.reference))
                {
                    usedPrefab = objectivePrefab;
                }

                GameObject objectiveInstance = Instantiate(usedPrefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();


                objectiveText.text = objective.description;

                rewardText.text = GetRewardText(quest);
            }
        }

        private string GetRewardText(Quest quest)
        {
            string rewardText = "";
            foreach (var reward in quest.GetRewards())
            {
                if (rewardText != "")
                {
                    rewardText += ", ";
                }
                if (reward.number > 1)
                {
                    rewardText += reward.number + " ";
                }
                rewardText += reward.item.GetDisplayName();
            }
            if (rewardText == "")
            {
                rewardText = "No Reward";
            }
            rewardText += ".";
            return rewardText;
        }
    }

}