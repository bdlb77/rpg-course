using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] progressionCharacterClasses = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            // get Correct Health From Correct CharacterClass from Corect Level
            foreach (ProgressionCharacterClass progCharClass in progressionCharacterClasses)
            {
                if (progCharClass.characterClass != characterClass) continue;

                foreach(ProgressionStat progressionStat in progCharClass.stats)
                {
                    if (progressionStat.stat != stat) continue;
                    if (progressionStat.levels.Length < level) continue; // if level we are searching for is out of bounds of len(levels)

                    return progressionStat.levels[level - 1];
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