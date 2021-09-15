using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;
        // Start is called before the first frame update
        private void OnDrawGizmos() {
           for (int i = 0; i < transform.childCount; i++)
           {
                int j = GetNextIndex(i);
                // position Vector of Child waypoints, Radius of Sphere
                Gizmos.DrawSphere(GetWayPoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
           }
        }
        public int GetNextIndex(int i)
        {
            if (i + 1 >= transform.childCount)
            {
                return 0;
            }

            return i + 1;
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }

    
    }

}