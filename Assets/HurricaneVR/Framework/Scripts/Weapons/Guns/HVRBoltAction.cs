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
    }
}
