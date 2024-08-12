using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HurricaneVR.Framework.Weapons.Guns
{
    public class HVRBoltAction : HVRGunBase
    {
        protected override void Awake()
        {
            base.Awake();

            ChambersAfterFiring = false;
            EjectCasingAfterFiring = false;
            BoltPushedBackAfterEmpty = false;
        }
        
        protected override bool CanFire()
        {
            if (CockingHandle is not HVRRotatingBoltHandle boltHandle) return true;
            //Check if rotating bolt handle is closed before we can fire
            if (boltHandle.rotationValue01 > .1f) return false;

            return base.CanFire();
        }

    }
}
