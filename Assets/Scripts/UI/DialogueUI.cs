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
            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(Next);
            // initialize UI w/ starting Text...
            UpdateUI();
        }

        void Next()
        {
            playerConversant.Next();
        }

        void UpdateUI()
        {
            if (!playerConversant.IsDialogueOpen())
            {
                return;
            }
            print("HEREEE3");

            AIResponse.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform choiceChild in choiceRoot)
            {
                Destroy(choiceChild.gameObject);
            }
            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComp.text = choice.Text;
                Button button = choiceInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => { 
                    playerConversant.SelectChoice(choice);
                });
            }
        }
    }

}