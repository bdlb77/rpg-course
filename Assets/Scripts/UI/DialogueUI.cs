using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI depends on Dialogue BUT dialogue DOES NOT depend on UI!s
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        // Start is called before the first frame update
        void Start()
        {
            playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();
            nextButton.onClick.AddListener(Next);
            // initialize UI w/ starting Text...
            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Next()
        {
            playerConversant.Next();
            UpdateUI();
        }
        void UpdateUI()
        {
            AIText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
        }
    }

}