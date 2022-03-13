using UnityEngine;
using RPG.Saving;
namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;


        public float GetExperiencePoints() 
        {
            return experiencePoints;
        }
        public void GainExperience(float experienceGained)
        {
            experiencePoints += experienceGained;
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