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

        public void Setup(Quest quest)
        {
            title.text = quest.Text;
            progress.text = "0/" + quest.GetObjectCount();
        }
    }

}