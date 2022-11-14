using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameDevTV.Inventories;
using RPG.Stats;

namespace RPG.Inventory
{
    public class StatsEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach(var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue;

                foreach (float modifier in item.GetAdditiveModifiers(stat))
                {
                    yield return modifier;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
              foreach(var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue;

                foreach (float modifier in item.GetPercentageModifiers(stat))
                {
                    yield return modifier;
                }
            }
        }

        // Start is called before the first frame update

    }

}
