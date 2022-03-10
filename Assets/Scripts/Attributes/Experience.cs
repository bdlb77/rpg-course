using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoints = 0f;

        public void GainExperience(float experienceGained)
        {
            experiencePoints += experienceGained;
        }
    }
}