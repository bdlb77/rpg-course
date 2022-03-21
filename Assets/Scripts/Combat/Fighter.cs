using RPG.Movement;
using UnityEngine;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        
        LazyValue<Weapon> currentWeapon;

        
        private void Awake() {
            currentWeapon = new LazyValue<Weapon>(InitializeDefaultWeapon);
        }
        private void Start()
        {
           currentWeapon.ForceInit();
        }

        private Weapon InitializeDefaultWeapon() {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Update()
        {
            // time it took last frame to render.. Add this delta each time to var
            timeSinceLastAttack += Time.deltaTime;

            // if target doesn't exist or is dead return
            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRage())
            {
                // 1f for full speed in fighting mode
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            // fighter knows if it needs to attack or not
            target = combatTarget.GetComponent<Health>();
            // move to target
            // stop a x distance
            // attack
        }

        public void Cancel()
        {
            StopAttack();
            GetComponent<Mover>().Cancel();
            target = null;
        }

        private bool GetIsInRage()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
        }

        private void StopAttack()
        {
            // reset Attack trigger  when Stopping Attack incase it is marked.
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack < timeBetweenAttacks) return;

            InitAttack();
            timeSinceLastAttack = 0;
        }

        private void InitAttack()
        {
            // reset stopAttack in instance that it may be set before initializing an attack on an enemy .
            GetComponent<Animator>().ResetTrigger("stopAttack");
            // This will trigger Hit() event
            GetComponent<Animator>().SetTrigger("Attack");
        }

        // Animation Event built in to Unity Animator
        private void Hit()
        {
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                // target is set in Attack() which is called in PlayerController 
                // target here is Health
                target.TakeDamage(gameObject, damage);

            }


        }

        // Animation Event
        private void Shoot()
        {
            Hit();
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
           if (stat == Stat.Damage)
           {
               yield return currentWeapon.value.GetDamage();
           }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetWeaponPercentageBonus();
            }
        }
        public bool CanAttack(GameObject combatTarget)
        {
            // If clicking not on a target
            if (combatTarget == null) return false;

            // If the target has a health component && is not Dead
            Health targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDead();
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }


    }
}