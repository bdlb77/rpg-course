using RPG.Movement;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f; // 2 meters

        [SerializeField] float timeBetweenAttacks = 1.1f;
        [SerializeField] float weaponDamage = 80f;
        Transform target;
        float timeSinceLastAttack = 0;

        Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()

        {
            // time it took last frame to render.. Add this delta each time to var
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (!GetIsInRage())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack < timeBetweenAttacks) return;

            // This will trigger Hit() event
            GetComponent<Animator>().SetTrigger("Attack");
            timeSinceLastAttack = 0;
        }
        // Animation Event built in to Unity Animator
        private void Hit()
        {
            // target is set in Attack() which is called in PlayerController 
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);

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