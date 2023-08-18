using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
public class QuestGiver : MonoBehaviour
{
    [SerializeField] Quest quest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveQuest()
    {
        var questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        questList.AddQuest(quest);
    }
}

}
