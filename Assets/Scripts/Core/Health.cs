using UnityEngine;


namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        bool isDead = false;
       
        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(float damage)
        {
            // take highest # between health - dmg || 0
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            GetComponent<Animator>().SetTrigger("Die");
            // Cancel Current Action since dead
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;
        }
    }
}