using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Mover : MonoBehaviour
{
    [SerializeField] Transform target; 
    // last Ray that was shot at the screen
    void Update()
    {
        // 0 => Left Button
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
    
        }
    }
}
