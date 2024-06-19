using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TDTK;


namespace TDTK
{
    public class UItdtk : MonoBehaviour
    {
        public static bool IsCursorOnUI(int inputID = -1)
        {
            EventSystem eventSystem = EventSystem.current;
            return (eventSystem.IsPointerOverGameObject(inputID));
        }

    }
}

