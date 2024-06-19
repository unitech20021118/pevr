using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data._data.action
{
    [Serializable]
    public class ShootingModeInforma:ActionInforma
    {
        public string GunName;
        public ShootingModeInforma(bool isOnce)
        {
            this.isOnce = isOnce;
        }
    }
}
