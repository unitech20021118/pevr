using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUp : Action<Main>
{
    public override void DoAction(Main m)
    {
        //m.gameObject.layer = 12;
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            if (Physics.RaycastAll(ray) != null)
            {
                hits = Physics.RaycastAll(ray);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform == m.transform)
                    {
                        even.Do();
                    }
                }
            }
        }
    }
}
