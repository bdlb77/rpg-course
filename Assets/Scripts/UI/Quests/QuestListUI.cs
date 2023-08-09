using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;
        // Start is called before the first frame update
        void Start()
        {
            transform.DetachChildren();
            var questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            foreach (var status in questList.GetStatuses())
            {
                var uiInstance = Instantiate<QuestItemUI>(questPrefab, transform);
                uiInstance.Setup(status);
            }
        }
    }

}