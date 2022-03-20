using RPG.Attributes;
using UnityEngine;
// 
namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        // 10 seconds;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] float lifeAfterImpact = 1;
        [SerializeField] GameObject[] destroyOnHit = null;
        Health target = null;
        GameObject instigator;
        float damage = 0;

        void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead()) // make sure homing missiles don't stick around to look at. 
            {
                transform.LookAt(GetAimLocation());
            }
            // transition delta and move at speed.. flowing at the correct frame rate. (make frame independent)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float weaponDamage)
        {
            this.target = target;
            this.damage = weaponDamage;
            this.instigator = instigator;

            // destroy this game object after maxLifeTime(10 seconds).
            Destroy(gameObject, maxLifeTime);
        }
        private Vector3 GetAimLocation()
        {
            // Use Capsule Collider of target to get Center of Mass
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return target.transform.position;

            //Get Target positon.. Then move up (Vector3.up) towards center of mass (targetCapsule.height / 2)
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return; // if enemy is dead.. Don't do damage
            print("TARGET: " + target);
            target.TakeDamage(instigator, damage);
            speed = 0;

            if (hitEffect != null)
            {

                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }

    }

}