using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_AdjustEditPanelPos : MonoBehaviour {

    Vector3 touchPos;

    public void SetEditorPanelPos()
    {
        touchPos = Input.mousePosition;
        AdjustPos();

    }

    private void AdjustPos()
    {
        RectTransform component=transform.GetComponent<RectTransform>();
        if (component != null)
        {
            touchPos.x = Input.mousePosition.x + component.sizeDelta.x/2;
            touchPos.y = Input.mousePosition.y - component.sizeDelta.y/2;

            if (Screen.width - Input.mousePosition.x < component.sizeDelta.x)
            {
                touchPos.x = Screen.width - component.sizeDelta.x;
            }
            if (Input.mousePosition.y < component.sizeDelta.y)
            {
                touchPos.y = component.sizeDelta.y;
            }
        }
        transform.position = touchPos;

    }
}
