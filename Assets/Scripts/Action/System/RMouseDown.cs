using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMouseDown : Action<Main>
{
    private Ray ray;
    private RaycastHit hit;
    public override void DoAction(Main m)
    {
        if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //Transform a=Manager.Instace.gonggong.transform.GetChild(0);
                Transform a = even.target.gameObject.transform;
                if (hit.collider.transform == a)
                {
                    even.Do();
                }
            }
        }
    }
}
