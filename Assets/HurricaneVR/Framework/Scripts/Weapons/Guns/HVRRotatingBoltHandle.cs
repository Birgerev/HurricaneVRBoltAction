using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Weapons.Guns;
using UnityEngine;
using UnityEngine.Serialization;

namespace HurricaneVR.Framework.Weapons.Guns
{
    public class HVRRotatingBoltHandle : HVRCockingHandle
    {
        public Vector3 handleRotationEulerAngles;
        public Vector3 boltRotationEulerAngles;
        public float RotateDifficulty = .05f;
        public Transform rotateUp;

        [HideInInspector] public float rotationValue01;
        private bool IsBoltLocked => rotationValue01 < .1f;
        private float _startingPullDifficulty;
        private Quaternion _startingLocalRotation;
        private Quaternion _startingLocalBoltRotation;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            _startingPullDifficulty = Difficulty;
            _startingLocalRotation = transform.localRotation;
            _startingLocalBoltRotation = Bolt.transform.localRotation;
        }

        protected override void Update()
        {
            base.Update();
            if (Grabbable.HandGrabbers.Count > 0)
            {
                var deltaHandMovement = (Grabbable.HandGrabbers[0].TrackedController.position -
                                         _grabbedPositionTracker.position);
                var localUpDirection = rotateUp.up.normalized * 10;
                var amount = Vector3.Dot(deltaHandMovement, localUpDirection) * RotateDifficulty;
                rotationValue01 += amount;
                rotationValue01 = Mathf.Clamp01(rotationValue01);

                if (Vector3.Distance(transform.position, ForwardPositionWorld) > .1f)
                    rotationValue01 = 1;

                transform.localRotation = Quaternion.Lerp(_startingLocalRotation,
                    Quaternion.Euler(handleRotationEulerAngles), rotationValue01);
                Bolt.transform.localRotation = Quaternion.Lerp(_startingLocalBoltRotation,
                    Quaternion.Euler(boltRotationEulerAngles), rotationValue01);
            }

            Difficulty = IsBoltLocked ? 0 : _startingPullDifficulty;
        }
/*
    protected override void CheckChamberDistance(float distance)
    {
        print($"rotation val {_lockedValue01}");
        //Check if bolt has been rotated closed
        if (_lockedValue01 > .1f)
        {
            _chamberDistanceReached = false;
            return;
        }
        
        print($"checking regular");
        base.CheckChamberDistance(distance);
    }*/
    }
}