using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Core;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string playerName;
        public event Action onConversationUpdated;

        AIConversant currentConversant;
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        private bool isChoosing = false;

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentConversant = null;
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
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
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
            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }
            DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool HasNext()
        {
            return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
        }

        public string GetCurrentConversantName()
        {
            if (isChoosing)
            {
                return playerName;
            }
            return currentConversant.ConversantName;
        }
        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        private void TriggerEnterAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.OnEnterAction);
            }
        }

        private void TriggerExitAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.OnExitAction);
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "") return;

            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }

        }

    }

}