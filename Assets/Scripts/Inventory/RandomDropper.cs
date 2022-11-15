using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventory
{

    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far can the pickups be scattered from the dropper.")]
        [SerializeField] float scatterDistance = 1;
        [SerializeField] DropLibrary dropLibrary;
        [SerializeField] int numberOfDrops = 2;
        // CONSTANTS
        const int ATTEMPTS = 30;

        
        public void RandomDrop()
        {
            for (int i = 0; i < numberOfDrops; i++)
            {
                var baseStats  = GetComponent<BaseStats>();

                var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
                foreach (var drop in drops)
                { 
                  DropItem(drop.item, drop.number);  
                }
            }

        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                // get randomPoint
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                // git hit fo NavMeshHit

                NavMeshHit hit;
                // check if randomPoint is on NavMesh

                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    // true = return randomPoint position
                    return hit.position;
                }
                // default return transform position (position of character)
            }

            return transform.position;
        }

    }

}