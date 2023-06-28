using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        // TODO: replace with functgion Call when clicking on AI
        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode = null;
        private bool isChoosing = false;

        private void Awake() {
            currentNode = currentDialogue.GetRootNode();
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
                return;
            }
            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = Random.Range(0, children.Count());
            currentNode =  children[randomIndex];
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }
    }

}