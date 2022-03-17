using UnityEngine;
using RPG.Saving;
using System;
namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;

        public event Action onExperienceGained;
        public float GetExperiencePoints() 
        {
            return experiencePoints;
        }
        public void GainExperience(float experienceGained)
        {
            experiencePoints += experienceGained;
            onExperienceGained();

        }
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}