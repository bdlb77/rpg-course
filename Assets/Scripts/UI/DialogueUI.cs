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
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;

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
            AIResponse.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            if (playerConversant.IsChoosing())
            {
                foreach (Transform choiceChild in choiceRoot)
                {
                    Destroy(choiceChild.gameObject);
                }
                foreach(DialogueNode choice in playerConversant.GetChoices())
                {
                    GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                    var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                    textComp.text = choice.Text;
                }
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }
    }

}