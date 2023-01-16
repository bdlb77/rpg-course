using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            Debug.Log("Awake from " + name);
            if (nodes.Count == 0)
            {
                DialogueNode rootNode = new DialogueNode();
                rootNode.uniqueID = Guid.NewGuid().ToString();
                nodes.Add(rootNode);
            }
            OnValidate();
        }

#endif
        private void OnValidate() {
            nodeLookup.Clear();
            foreach(DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.uniqueID] = node;
            }
        }
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            // COMMENTED CODE IS BASICALLY THE SAME AS THE BOTTOM

            // List<DialogueNode> foundNodes = new List<DialogueNode>();
            // foreach(string childID in parentNode.children)
            // {
            //     if (nodeLookup.ContainsKey(childID))
            //     {
            //         foundNodes.Add(nodeLookup[childID]);
            //     }
            // }
            
            // return foundNodes;

            
            foreach(string childID in parentNode.children)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    // can yield return a single item to build up our IEnumerable .
                    yield return nodeLookup[childID];
                }
            }
        }

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = new DialogueNode();
            newNode.uniqueID = Guid.NewGuid().ToString();
            parent.children.Add(newNode.uniqueID);
            nodes.Add(newNode);

            // update the onLookup to redraw bezier lines
            OnValidate();

        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildrenNodes(nodeToDelete);
        }

        private void CleanDanglingChildrenNodes(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.children.Remove(nodeToDelete.uniqueID);
            }
        }
    }

}