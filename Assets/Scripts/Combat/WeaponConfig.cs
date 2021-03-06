using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] float weaponDamage = 80f;
        [SerializeField] float weaponPercentageBonus = 0;
        [SerializeField] float weaponRange = 2f; // 2 meters
        [SerializeField] bool isRightHanded = true;

        const string weaponName = "Weapon";
        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon = null;
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null) {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                // so.. will be "null" if animator is the ROOT Animator Controller.. Else it will be override Animator .
                // E.g., override = Fireball Animator.. Root = Punch. If overide is null.. cast to override won't work and overrideController will default to Root.
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null) {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;
            
            // make sure we know that old weapon is DESTROYING
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform righthand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            // Quaternion.identity = orientation (rotation E.g.) of object in world
            Projectile projectileInstance = Instantiate(projectile, GetTransform(righthand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }
        public float GetDamage() {
            return weaponDamage;
        }

        public float GetRange() {
            return weaponRange;
        }

        public float GetWeaponPercentageBonus()
        {
            return weaponPercentageBonus;
        }
        Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }
    }
}