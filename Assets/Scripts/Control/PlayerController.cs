using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if (InteractWithCombat()) return;
            if(InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        // Return True if hit for combat is found
        {
            Ray ray = GetMouseRay();
            RaycastHit[] hits = Physics.RaycastAll(ray);
            // Loop through all raycast hits
            // if a raycast hit has component of CombatTarget
                // if Left Click
                    // Attack from Fighter
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
                }
                return true;

            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InteractWithMovement()
        // If true, means that Raycast hit something on scene. Else return false (clicking on edge of world)
        {

            Ray ray = GetMouseRay();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {

                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;

            }
            return false;
        }

    }

}