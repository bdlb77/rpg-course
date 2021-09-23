using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
// 
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Health target = null;
    float damage = 0;

    void Update()
    {
        if (target == null) return;

        transform.LookAt(GetAimLocation());
        // transition delta and move at speed.. flowing at the correct frame rate. (make frame independent)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health target, float weaponDamage)
    {
        this.target = target;
        this.damage = weaponDamage;
    }
    private Vector3 GetAimLocation()
    {
        // Use Capsule Collider of target to get Center of Mass
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if(targetCapsule == null) return target.transform.position;

        //Get Target positon.. Then move up (Vector3.up) towards center of mass (targetCapsule.height / 2)
        return target.transform.position + Vector3.up  * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other) {
        print("Here!!!");
        if (other.GetComponent<Health>() != target) return;
        print(damage);
        target.TakeDamage(damage);
        Destroy(gameObject);
    }

}
