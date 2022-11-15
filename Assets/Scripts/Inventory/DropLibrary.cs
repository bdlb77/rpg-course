using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Inventory
{

    [CreateAssetMenu(menuName = ("RPG/Inventory/Drop Library"))]
    public class DropLibrary : ScriptableObject
    {
        [SerializeField]
        DropConfig[] potentialDrops;
        // array gives us a percentage that correlates to level (idx position in lvl array)
        [SerializeField] float[] dropChancePercentage;
        [SerializeField] int[] minDrops;
        [SerializeField] int[] maxDrops;

        
        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float[] relativeChance;
            public int[] minNumber;
            public int[] maxNumber;
            public int GetRandomNumber(int level)
            {
                if (!item.IsStackable())
                {
                    return 1;
                }
                int min = GetByLevel(minNumber, level);
                int max = GetByLevel(maxNumber, level);
                return UnityEngine.Random.Range(min, max + 1);
            }
        }

        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }

        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldRandomDrop(level))
            {
                yield break;
            }

            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }

        }
        DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = UnityEngine.Random.Range(0, totalChance);
            float chanceTotal = 0;
            foreach (var drop in potentialDrops)
            {
                chanceTotal += GetByLevel(drop.relativeChance, level);
                if (chanceTotal > randomRoll)
                {
                    return drop;
                }
            }
            return null;
        }

        Dropped GetRandomDrop(int level)
        {
            // select random item suitable for level of enemy to be dropped
            var drop = SelectRandomItem(level);
            // create new dropped struct
            var result = new Dropped();
            
            result.item = drop.item;
            // get random number of items to be dropped.
            result.number = drop.GetRandomNumber(level);

            return result;
        }

        int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(minDrops, level);
            int max = GetByLevel(maxDrops, level);
            return UnityEngine.Random.Range(min, max);
        }

        bool ShouldRandomDrop(int level)
        {
            // dropChancePercentage will be percentage to drop.. (between 1-100)
            // we roll dice (100-sided) to see if our roll is lower than the drop percentage.
            // if Lower, we will allow item to drop.
            return UnityEngine.Random.Range(0, 100) < GetByLevel(dropChancePercentage, level);
        }


        private float GetTotalChance(int level)
        {
            float total = 0;
            foreach (var drop in potentialDrops)
            {
                total += GetByLevel(drop.relativeChance, level);
            }
            return total;
        }

        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
            {
                return default;
            }
            if (level > values.Length)
            {
                return values[values.Length - 1];
            }
            if (level <= 0)
            {
                return default;
            }

            return values[level - 1];
        }
    }
}
