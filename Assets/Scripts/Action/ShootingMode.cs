using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Action
{
    public class ShootingMode:Action<Main>
    {
        public string GunName;
        public override void DoAction(Main m)
        {
            if (IsSetFirstPerson())
            {
                Manager.Instace.LoadShootingMode(even);
                
            }
        }

        private bool IsSetFirstPerson()
        {
            return Manager.Instace.FirstPerson.activeSelf;
        }
    }
}
