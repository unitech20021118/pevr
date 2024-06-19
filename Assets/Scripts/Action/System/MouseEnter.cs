using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnter : Action<Main>
{
    private RaycastHit lastHit;
    private Ray ray;
    private RaycastHit hit;
    public override void DoAction(Main m)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == m.gameObject)
            {
                if (lastHit.collider == null)
                {
                    even.Do();
                }
                else
                {
                    if (lastHit.collider.gameObject != m.gameObject)
                    {
                        even.Do();
                    }
                }
            }
            lastHit = hit;
        }
    }
}
