using System;
using UnityEngine;
using UniHumanoid;

namespace Sign
{
    public class BodyController
    {
        private Transform _spine;
        private Transform _chest;
        private Transform _upperChest;
        private Transform _neck;
        private Transform _head;
        private Transform _rShoulder;
        private Transform _lShoulder;

        private Quaternion _restSpine;
        private Quaternion _restUpperChest;
        private Quaternion _restNeck;
        private Quaternion _restRShoulder;
        private Quaternion _restLShoulder;

        private BodyRotation _bodyRotation;
        
        private const float BreathingSpeed = 2;
        private const float BreathingPower = 80;
        private float _t;
        
        public BodyController(Humanoid humanoid)
        {
            
            Populate(humanoid);
        }
        private void Populate(Humanoid humanoid)
        {
            _spine = humanoid.Spine;
            _chest = humanoid.Chest;
            _upperChest = humanoid.UpperChest;
            _neck = humanoid.Neck;
            _head = humanoid.Head;
            _rShoulder = humanoid.RightShoulder;
            _lShoulder = humanoid.LeftShoulder;
            
            _restSpine = _spine.localRotation;
            _restUpperChest = _upperChest.localRotation;
            _restNeck = _neck.localRotation;
            _restRShoulder = _rShoulder.localRotation;
            _restLShoulder = _lShoulder.localRotation;
        }
        
        public void Update(BodyRotation bodyRotation, float t, bool inGesture)
        {
            _bodyRotation = bodyRotation;
            if (inGesture) SpineRotation(t);
            else Idle();
        }

        private void SpineRotation(float t)
        {
            SmoothMotion(_spine, _bodyRotation.Spine, t);
        }
        
        private void SmoothMotion(Transform hum, Quaternion rotation, float t)
        {
            hum.rotation = Quaternion.Slerp(hum.rotation, rotation, t);
        }

        private void Idle()
        {
            _spine.rotation = Quaternion.Slerp(_spine.rotation, _restSpine, Time.deltaTime * 3);
            // BreathingChest();
        }
        

        private void BreathingChest()
        {
            _upperChest.localRotation = Quaternion.Euler(
                Mathf.Sin(_t / BreathingSpeed * Mathf.PI) * (Mathf.PI / BreathingPower) * Mathf.Rad2Deg,
                _upperChest.localRotation.eulerAngles.y,
                _upperChest.localRotation.eulerAngles.z
            ) * _restUpperChest;
            
            _neck.localRotation = Quaternion.Inverse(Quaternion.Euler(
                Mathf.Sin(_t / BreathingSpeed * Mathf.PI) * (Mathf.PI / BreathingPower) * Mathf.Rad2Deg,
                _neck.localRotation.eulerAngles.y,
                _neck.localRotation.eulerAngles.z
            )) * _restNeck;
        }
        
    }
    
    
    
}