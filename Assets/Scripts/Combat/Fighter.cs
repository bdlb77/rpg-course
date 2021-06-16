using RPG.Movement;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f; // 2 meters
        Transform target;

        Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        
        {
            if (target == null) return;

            if (!GetIsInRage())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
            }
        }

        private bool GetIsInRage()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            // fighter knows if it needs to attack or not
            target = combatTarget.transform;
            print("Take that you PEASANT!");
            // move to target
            // stop a x distance
            // attack
        }
        
        public void Cancel()
        {
            target = null;
        }
    }
}