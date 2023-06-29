using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        public event Action onConversationUpdated;

        // TODO: replace with functgion Call when clicking on AI
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        private bool isChoosing = false;

        public void StartDialogue(Dialogue newDialogue)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }
        public bool IsDialogueOpen()
        {
            return currentDialogue != null;
        }

        // show choices or show AI response.
        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null)
            {
                return "";
            }

            return currentNode.Text;
        }
        
        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            // comment out Next(), will read out text of chosen answer before moving onto the next node.
            // If Next() is active, then it will go straight to next node.
            Next();
        }
        
        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }
            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode =  children[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        private void TriggerEnterAction()
        {
            if (currentNode != null && currentNode.OnEnterAction != "")
            {
                Debug.Log(currentNode.OnEnterAction);
            }
        }
        private void TriggerExitAction()
        {
            if (currentNode != null && currentNode.OnExitAction != "")
            {
                Debug.Log(currentNode.OnExitAction);
            }
        }
    }

}