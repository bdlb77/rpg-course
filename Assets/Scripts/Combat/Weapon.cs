using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 80f;
        [SerializeField] float weaponRange = 2f; // 2 meters

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride != null) {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
        public float GetDamage() {
            return weaponDamage;
        }

        public float GetRange() {
            return weaponRange;
        }
    }
}