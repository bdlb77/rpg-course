using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(0, 0, 200, 100);

        [SerializeField] bool isPlayerSpeaking = false;

        public Rect GetRect { get { return rect; } }
        public string Text { get { return text; } }
        public List<string> Children { get { return children; } }
        public bool IsPlayerSpeaking { get { return isPlayerSpeaking; }}
#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            // make sure the dialogue Node is marked as dirty in scene since if Child Object, we need to mark it dirty to save properly
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {

            if (text != newText)
            {
                Undo.RecordObject(this, "Update Dialogue Text");                
                text = newText;
                // make sure the dialogue Node is marked as dirty in scene since if Child Object, we need to mark it dirty to save properly
                EditorUtility.SetDirty(this);

            }
        }

        public void AddChild(string newChild)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(newChild);
            // make sure the dialogue Node is marked as dirty in scene since if Child Object, we need to mark it dirty to save properly
            EditorUtility.SetDirty(this);

        }

        public void RemoveChild(string childToRemove)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(childToRemove);
            // make sure the dialogue Node is marked as dirty in scene since if Child Object, we need to mark it dirty to save properly
            EditorUtility.SetDirty(this);
        }

        internal void SetPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Is Player Speaking");
            isPlayerSpeaking = newIsPlayerSpeaking;
            EditorUtility.SetDirty(this);

        }
    }
#endif
}