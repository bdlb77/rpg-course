using System;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startinglevel = 1;
        [SerializeField] CharacterClass characterClass;
        // Start is called before the first frame update
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifiers = false;
        LazyValue<int> currentLevel;

        public event Action onLevelUp;
        Experience experience;
        private void Awake()
        {
            Experience experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }
        private void Start()
        {
           currentLevel.ForceInit();
        }


        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                // call on subscribed functions to `onLevelUp`
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            // set off of transform of parent(Player)
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }


        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }
        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startinglevel;

            float currentXP = experience.GetExperiencePoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToNextLevel, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToNextLevel, characterClass, level);
                if (xpToLevelUp > currentXP)
                {
                    return level;
                }
            }
            // if no level XP are greater than current leel .. Then must be ultimate level (highest level)
            return penultimateLevel + 1;
        }
        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

    }

}