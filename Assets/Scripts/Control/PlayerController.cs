using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;

using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

    
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;

        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxNavPathLength = 40f;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            };
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            // Raycast through world and get all hits.. and get
            RaycastHit[] hits = RacycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithUI()
        {
            // Return True / false if we are hovering over Piece of UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }


        private bool InteractWithMovement()
        // If true, means that Raycast hit something on scene. Else return false (clicking on edge of world)
        {

            // Ray ray = GetMouseRay();
            // RaycastHit hit;
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            // bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {

                    // 1f means 100% movement speed of MaxSpeed
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;            
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            
            // have not been able to find a nearby navMesh point that was hit by the cursor..
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            
            NavMeshPath path = new NavMeshPath();
			bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path); 
            if (!hasPath) return false;
            // If path is not a complete Path
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float totalLength = 0f;
            if (path.corners.Length < 2) return totalLength;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                totalLength += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return totalLength;
        }

        private RaycastHit[] RacycastAllSorted()
        {
              RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
                float[] distances = new float[hits.Length];
                for (int i = 0; i < hits.Length; i++) {
                    distances[i] = hits[i].distance;
                }
                Array.Sort(distances, hits);
                return hits;

        }
        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            //
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == cursorType)
                {
                    return mapping;
                }
            }
            // just return first if doesn't exist
            return cursorMappings[0];
        }
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }


    }

}