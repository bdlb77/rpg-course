using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue.Editor 
{

    public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            Dialogue dialog = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;
            if (dialog == null) return AssetMoveResult.DidNotMove;

            // if the file is moving to a different directory, then we don't care.
            // dileneates a moving of a directory rather than just a rename that we actually care about
            if (Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath))
             {
                return AssetMoveResult.DidNotMove;
             }
            
            // Now there is a rename only
            // rename dialogue
            dialog.name = Path.GetFileNameWithoutExtension(destinationPath);


            // we are not manually handling the move of the file in the Asset DB
            return AssetMoveResult.DidNotMove;
        }
    }

}