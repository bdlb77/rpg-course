using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
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
            // laid out like Display Block
            EditorGUILayout.LabelField("Hello World");
            EditorGUILayout.LabelField("Hello World1");
            EditorGUILayout.LabelField("Hello World2");
        }
    }

}