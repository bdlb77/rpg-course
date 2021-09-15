using System;
using UnityEngine;

namespace RPG.Core
{

    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;

        // normal variable lives / dies with instance of Class. static lives / dies with APPLICATION (not garbage collected..)
        static bool hasSpawned = false;
        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObject();
            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}