using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Dialogue;
using UnityEngine;

public class AIConversant : MonoBehaviour, IRaycastable
{
    [SerializeField] Dialogue dialogue = null;
    public CursorType GetCursorType()
    {
        return CursorType.Dialogue;
    }

    public bool HandleRaycast(PlayerController callingController)
    {
        if (dialogue == null)
        {
            return false;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
        }
        return true;
    }

}
