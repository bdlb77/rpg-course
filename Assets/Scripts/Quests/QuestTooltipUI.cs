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
        public void Setup(Quest quest)
        {
            title.text = quest.Title;
            objectiveContainer.DetachChildren();

            foreach(var objective in quest.GetObjectives())
            {
                GameObject objectiveInstance = Instantiate(objectivePrefab,objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective;

            }
        }
    }

}