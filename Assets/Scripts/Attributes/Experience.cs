using UnityEngine;
using RPG.Saving;
namespace RPG.Attributes
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;


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