using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseExit : Action<Main>
{
    private RaycastHit lastHit;
    private Ray ray;
    private RaycastHit hit;
    public override void DoAction(Main m)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (!Physics.Raycast(ray,out hit))
        {
            if (lastHit.collider != null && lastHit.collider.gameObject == m.gameObject)
            {
                even.Do();
            }
        }
        else
        {
            if (hit.collider.gameObject != m.gameObject && lastHit.collider.gameObject == m.gameObject)
            {
                even.Do();
            }
            lastHit = hit;
        }
    }
}

