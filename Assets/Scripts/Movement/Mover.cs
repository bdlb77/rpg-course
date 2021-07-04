
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{

    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
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

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;

        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);

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
    }
}
