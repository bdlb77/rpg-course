using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startinglevel = 1;
        [SerializeField] CharacterClass characterClass;
        // Start is called before the first frame update
        [SerializeField] Progression progression = null;

        int currentLevel = 0;

        private void Start() {
            currentLevel = CalculateLevel();
        }
        private void Update() {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                print("Leveled Up!");
            }
        }
        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel;
        }
        public int CalculateLevel()
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
    }

}