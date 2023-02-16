using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();



        private void OnValidate() {
            nodeLookup.Clear();
            foreach(DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
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

            
            foreach(string childID in parentNode.Children)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    // can yield return a single item to build up our IEnumerable .
                    yield return nodeLookup[childID];
                }
            }
        }

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            // create new Scriptable Object. 
            DialogueNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");
            AddNode(newNode);

        }


        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleting Dialogue Node");

            nodes.Remove(nodeToDelete);

            OnValidate();
            CleanDanglingChildrenNodes(nodeToDelete);
            // need to destroy after cleanDanglingChildren since we use it's reference to clean any chjildren nodes
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildrenNodes(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }


        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            // update the onLookup to redraw bezier lines
            OnValidate();
        }
        
        private static DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parent != null)
            {
                parent.AddChild(newNode.name);
            }

            return newNode;
        }
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            // When about to save, make sure we check to create root node
            if (nodes.Count == 0)
            {
                CreateNode(null);
            }
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            Debug.Log("Not Implemented");
        }
    }

}