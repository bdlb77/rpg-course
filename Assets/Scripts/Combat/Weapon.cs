using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] float weaponDamage = 80f;
        [SerializeField] float weaponRange = 2f; // 2 meters
        [SerializeField] bool isRightHanded = true;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);

                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride != null) {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform righthand, Transform leftHand, Health target)
        {
            // Quaternion.identity = orientation (rotation E.g.) of object in world
            Projectile projectileIsntance = Instantiate(projectile, GetTransform(righthand, leftHand).position, Quaternion.identity);
            projectileIsntance.SetTarget(target, weaponDamage);
        }
        public float GetDamage() {
            return weaponDamage;
        }

        public float GetRange() {
            return weaponRange;
        }

        Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }
    }
}