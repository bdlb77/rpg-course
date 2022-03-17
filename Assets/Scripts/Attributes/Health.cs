using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70f;
        float healthPoints = -1f;

        bool isDead = false;


        private void Start()
        {
            // If starting.. then hp = -1.. Meaning it's a situation that will not happen in game... therefore the beginning
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if (healthPoints < 0)
            {
                // get health from Base Stats and Progression.
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }


        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            // take highest # between health - dmg || 0
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            float maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            return 100 * healthPoints / maxHealth;

        }
        public object CaptureState()
        {
            // return serializable object
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            float savedHealth = (float)state;

            healthPoints = savedHealth;

            if (healthPoints == 0) Die();
        }

        void Die()
        {
            if (isDead) return;

            GetComponent<Animator>().SetTrigger("Die");
            // Cancel Current Action since dead
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;

        }

        private void AwardExperience(GameObject instigator)
        {
            // check if instigator rewards Experience
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));

        }
        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            // Regen either to healthPoints OR 70% of new level's points.
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }
    }
}