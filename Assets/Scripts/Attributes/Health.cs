using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;
using GameDevTV.Utils;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70f;
        LazyValue<float> healthPoints;

        bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void Start()
        {
            healthPoints.ForceInit();
            // If starting.. then hp = -1.. Meaning it's a situation that will not happen in game... therefore the beginning
            // if (healthPoints < 0)
            // {
            //     // get health from Base Stats and Progression.
            //     healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            // }
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " Took Damage: " + damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            print(healthPoints.value);
            if (healthPoints.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
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
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            // return serializable object
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            float savedHealth = (float)state;

            healthPoints.value = savedHealth;

            if (healthPoints.value == 0) Die();
        }
    }
}