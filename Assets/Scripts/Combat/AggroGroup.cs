using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
{
    [SerializeField] Fighter[] fighters;
    [SerializeField] bool activateOnStart = false;
    
    private void Start() {
        Active(activateOnStart);
    }
    public void Active(bool shouldActivate)
    {
        foreach(Fighter fighter in fighters)
        {
            CombatTarget target = fighter.GetComponent<CombatTarget>();
            if (target != null)
            {
                target.enabled = shouldActivate;
            }
            fighter.enabled = shouldActivate;
        }
    }
}

}