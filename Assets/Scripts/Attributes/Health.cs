using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;


        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            // take highest # between health - dmg || 0
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            if (healthPoints == 0)
            {
                Die();
            }
        }

        public float GetPercentage()
        {
            float maxHealth = GetComponent<BaseStats>().GetHealth();
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
        private void Start()
        {
            // get health from Base Stats and Progression.
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }
    }
}