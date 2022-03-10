using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] progressionCharacterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            // get Correct Health From Correct CharacterClass from Corect Level
            foreach (ProgressionCharacterClass progCharClass in progressionCharacterClasses)
            {
                if (progCharClass.characterClass == characterClass)
                {
                    // level - 1 for index
                    return progCharClass.health[level - 1];
                }
            }
            return 0;
        }
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public  CharacterClass characterClass;
            public  ProgressionStat[] stats;

        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;

        }
    }
}