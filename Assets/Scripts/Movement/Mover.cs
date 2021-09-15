
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{

    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        // last Ray that was shot at the screen

        NavMeshAgent navMeshAgent;
        Health health;
        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;

        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);

        }


        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            // InverseTransformDirection ... Taking global and convert to local
            // When creating velocity we are grabbing global velocity (x,y in global)... Want to know what this character is moving at in terms of velocity.. Am I moving forward / etc relative to the local object (character)
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            // we want to capture position for state (For saving). Must Serialize Vector3 as Class with instance variables for x,y,z
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            // assume same object we return in `CaptureSave()` is exactly same object we will use as our argument in `RestoreState()`
            // Cast SerializeableVector3 
            SerializableVector3 savedPosition = (SerializableVector3)state;

            // sometimes weird conditions with NavMeshAgent. Stops NavMeshAgent from meddling with our position
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = savedPosition.ToVector(); 
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
