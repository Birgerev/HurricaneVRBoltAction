using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Weapons.Guns;
using UnityEngine;
using UnityEngine.Serialization;

public class HVRRotatingBoltHandle : HVRCockingHandle
{
    public Vector3 handleRotationEulerAngles;
    public Vector3 boltRotationEulerAngles;
    public float RotateDifficulty = .05f;
    public Transform rotateUp;

    private float _lockedValue01;
    private bool IsBoltLocked => _lockedValue01 < .1f;
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
            var deltaHandMovement = (Grabbable.HandGrabbers[0].TrackedController.position - _grabbedPositionTracker.position);
            var localUpDirection = rotateUp.up.normalized * 10;
            var amount = Vector3.Dot(deltaHandMovement, localUpDirection) * RotateDifficulty;
            _lockedValue01 += amount;
            _lockedValue01 = Mathf.Clamp01(_lockedValue01);

            //if (Vector3.Distance(transform.position, ForwardPositionWorld) > .05f)
            //    _lockedValue01 = 1;
            
            transform.localRotation = Quaternion.Lerp(_startingLocalRotation, Quaternion.Euler(handleRotationEulerAngles), _lockedValue01);
            Bolt.transform.localRotation = Quaternion.Lerp(_startingLocalBoltRotation, Quaternion.Euler(boltRotationEulerAngles), _lockedValue01);
        }

        Difficulty = IsBoltLocked ? 0 : _startingPullDifficulty;
    }


    /*protected virtual void OnGrabbed(HVRHandGrabber grabber, HVRGrabbable slide)
    {
        
    }*/

    protected override void OnReleased(HVRHandGrabber grabber, HVRGrabbable slide)
    {
        //EmptyOpen = false;
    }
}
