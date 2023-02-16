using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;

        [NonSerialized]
        GUIStyle nodeStyle;
        [NonSerialized]
        GUIStyle playerNodeStyle;

        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        DialogueNode draggingNode;
        [NonSerialized]
        DialogueNode creatingNode;

        [NonSerialized]
        DialogueNode deletingNode;
        [NonSerialized]
        DialogueNode linkingParentNode = null;
        
        Vector2 scrollPosition;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;

        const float canvasSize = 4000;
        const float backgroundSize = 50;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAssetAttribute(1)]
        public static bool onOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;

            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }
        public void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected", EditorStyles.whiteLabel);
            }
            else
            {
                ProcessEvents();
                // connections will go underneath nodes
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
                Rect textCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textCoords);

                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }

                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;

                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect.position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else 
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;

                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                // update scrollPosition
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                // redraw
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 mousePosition)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect.Contains(mousePosition))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }

        private void DrawNode(DialogueNode node)
        {
            GUIStyle style = nodeStyle;
            if (node.IsPlayerSpeaking)
            {
                style = playerNodeStyle;
            }
            
            GUILayout.BeginArea(node.GetRect, style);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Node: ");
            string newText = EditorGUILayout.TextField(node.Text);

            if (EditorGUI.EndChangeCheck())
            {
                node.SetText(newText);
            }

            DrawLinkButtons(node);
            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                Debug.Log("Creating new Node");
                creatingNode = node;
            }

            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else
            {
                if (linkingParentNode.Children.Contains(node.name))
                {
                    if (GUILayout.Button("unlink"))
                    {
                        linkingParentNode.RemoveChild(node.name);
                        linkingParentNode = null;

                    }
                }
                else
                {
                    if (GUILayout.Button("child"))
                    {
                        linkingParentNode.AddChild(node.name);
                        // allows toggle between link & child.
                        linkingParentNode = null;
                    }
                }
            }
            if (GUILayout.Button("-"))
            {
                Debug.Log("Deleting  Node");
                deletingNode = node;
            }
            GUILayout.EndHorizontal();
        }

        private void DrawConnections(DialogueNode parentNode)
        {
            Vector3 startPosition = new Vector2(parentNode.GetRect.xMax, parentNode.GetRect.center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(parentNode))
            {

                Vector3 endPosition = new Vector2(childNode.GetRect.xMin, childNode.GetRect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(
                    startPosition,
                    endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white,
                    null,
                    4f);
            }
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;

            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            
            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.normal.textColor = Color.white;

            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue == null) return;

            selectedDialogue = newDialogue;
        }
    }

}