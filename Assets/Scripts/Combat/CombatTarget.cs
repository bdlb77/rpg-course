using UnityEngine;
using RPG.Core;
using RPG.Attributes;

namespace RPG.Combat
{
    // When we place Combat Target, it will place Health COmponent
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
    
    }
}