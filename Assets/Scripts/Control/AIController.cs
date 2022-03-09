
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f; // 3 second!
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wpPromixityTolerance = 1f;
        [SerializeField] float waypointDwellTime = 1.5f;
        
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        Fighter fighter;
        GameObject player;

        Health health;
        Mover mover;

        // Starting position of the guard
        Vector3 guardPosition;
        // Guard memory! Infinity since the time initially should be really high!  SO when check and updating. .it will always be lower on Initial LOS of player
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceAtWaypoint = Mathf.Infinity;
        int currrentWayPointIdx = 0;
        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;

        }
        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();

            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();

            }
            else
            {
                PatrolBehaviour();

            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition; // default;
            
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceAtWaypoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }
            if (timeSinceAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWaypoint < wpPromixityTolerance;
        }

        private void CycleWayPoint()
        {
            // update to next index
            currrentWayPointIdx = patrolPath.GetNextIndex(currrentWayPointIdx);
        }
        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPoint(currrentWayPointIdx);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            // reset time since last saw back to 0;
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        // called by United
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);

        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }


    }

}