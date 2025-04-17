using System;
using UniHumanoid;
using UnityEngine;

namespace Sign
{
    public class Hum
    {
        #region hum
        private Transform _upperArm;
        private Transform _lowerArm;
        private Transform _hand;
        private Transform _index1;
        private Transform _index2;
        private Transform _index3;
        private Transform _little1;
        private Transform _little2;
        private Transform _little3;
        private Transform _middle1;
        private Transform _middle2;
        private Transform _middle3;
        private Transform _ring1;
        private Transform _ring2;
        private Transform _ring3;
        private Transform _thumb1;
        private Transform _thumb2;
        private Transform _thumb3;
        #endregion
        
        #region get
        public Transform UpperArm() => _upperArm;
        public Transform LowerArm() => _lowerArm;
        public Transform Hand() => _hand;
        public Transform Index1() => _index1;
        public Transform Index2() => _index2;
        public Transform Index3() => _index3;
        public Transform Little1() => _little1;
        public Transform Little2() => _little2;
        public Transform Little3() => _little3;
        public Transform Middle1() => _middle1;
        public Transform Middle2() => _middle2;
        public Transform Middle3() => _middle3;
        public Transform Ring1() => _ring1;
        public Transform Ring2() => _ring2;
        public Transform Ring3() => _ring3;
        public Transform Thumb1() => _thumb1;
        public Transform Thumb2() => _thumb2;
        public Transform Thumb3() => _thumb3;
        #endregion
        
        public Hum(Humanoid humanoid, Side side)
        {
            Populate(humanoid, side);
        }
        
        private void Populate(Humanoid humanoid, Side side)
        {
            _upperArm = side == Side.Right ? humanoid.RightUpperArm : humanoid.LeftUpperArm;
            _lowerArm = side == Side.Right ? humanoid.RightLowerArm : humanoid.LeftLowerArm;
            _hand = side == Side.Right ? humanoid.RightHand : humanoid.LeftHand;
            _index1 = side == Side.Right ? humanoid.RightIndexProximal : humanoid.LeftIndexProximal;
            _index2 = side == Side.Right ? humanoid.RightIndexIntermediate : humanoid.LeftIndexIntermediate;
            _index3 = side == Side.Right ? humanoid.RightIndexDistal : humanoid.LeftIndexDistal;
            _little1 = side == Side.Right ? humanoid.RightLittleProximal : humanoid.LeftLittleProximal;
            _little2 = side == Side.Right ? humanoid.RightLittleIntermediate : humanoid.LeftLittleIntermediate;
            _little3 = side == Side.Right ? humanoid.RightLittleDistal : humanoid.LeftLittleDistal;
            _middle1 = side == Side.Right ? humanoid.RightMiddleProximal : humanoid.LeftMiddleProximal;
            _middle2 = side == Side.Right ? humanoid.RightMiddleIntermediate : humanoid.LeftMiddleIntermediate;
            _middle3 = side == Side.Right ? humanoid.RightMiddleDistal : humanoid.LeftMiddleDistal;
            _ring1 = side == Side.Right ? humanoid.RightRingProximal : humanoid.LeftRingProximal;
            _ring2 = side == Side.Right ? humanoid.RightRingIntermediate : humanoid.LeftRingIntermediate;
            _ring3 = side == Side.Right ? humanoid.RightRingDistal : humanoid.LeftRingDistal;
            _thumb1 = side == Side.Right ? humanoid.RightThumbProximal : humanoid.LeftThumbProximal;
            _thumb2 = side == Side.Right ? humanoid.RightThumbIntermediate : humanoid.LeftThumbIntermediate;
            _thumb3 = side == Side.Right ? humanoid.RightThumbDistal : humanoid.LeftThumbDistal;
        }
        
        public Transform GetHum(int index)
        {
            return index switch
            {
                0 => _upperArm,
                1 => _lowerArm,
                2 => _hand,
                3 => _index1,
                4 => _index2,
                5 => _index3,
                6 => _little1,
                7 => _little2,
                8 => _little3,
                9 => _middle1,
                10 => _middle2,
                11 => _middle3,
                12 => _ring1,
                13 => _ring2,
                14 => _ring3,
                15 => _thumb1,
                16 => _thumb2,
                17 => _thumb3,
                _ => throw new IndexOutOfRangeException("Invalid rotation index")
            };
        }
        
        public void SetRotation(int index, Quaternion rotation)
        {
            switch (index)
            {
                case 0: RotateHum(_upperArm, rotation); break;
                case 1: RotateHum(_lowerArm, rotation); break;
                case 2: RotateHum(_hand, rotation); break;
                case 3: RotateHum(_index1, rotation); break;
                case 4: RotateHum(_index2, rotation); break;
                case 5: RotateHum(_index3, rotation); break;
                case 6: RotateHum(_little1, rotation); break;
                case 7: RotateHum(_little2, rotation); break;
                case 8: RotateHum(_little3, rotation); break;
                case 9: RotateHum(_middle1, rotation); break;
                case 10: RotateHum(_middle2, rotation); break;
                case 11: RotateHum(_middle3, rotation); break;
                case 12: RotateHum(_ring1, rotation); break;
                case 13: RotateHum(_ring2, rotation); break;
                case 14: RotateHum(_ring3, rotation); break;
                case 15: RotateHum(_thumb1, rotation); break;
                case 16: RotateHum(_thumb2, rotation); break;
                case 17: RotateHum(_thumb3, rotation); break;
                
                default: throw new IndexOutOfRangeException("Invalid rotation index");
            }
        }
        
        private void RotateHum(Transform hum, Quaternion rotation)
        {
            hum.rotation = rotation;
        }

        private FrameRotation _rotation;
        private float _t;
        public void UpdateHum(FrameRotation rotation, float t)
        {
            _rotation = rotation;
            if (!rotation.CheckHand) return;
            _t = t;
            UpdateArm();
            UpdateHand();
            UpdateFingers();
        }

        private void UpdateArm()
        {
            SmoothMotion(_upperArm, _rotation.UpperArm);
            SmoothMotion(_lowerArm, _rotation.LowerArm);
        }

        private void UpdateHand()
        {
            SmoothMotion(_hand, _rotation.Hand);
        }

        private void UpdateFingers()
        {
            SmoothMotion(_index1, _rotation.Index1);
            SmoothMotion(_index2, _rotation.Index2);
            SmoothMotion(_index3, _rotation.Index3);
            SmoothMotion(_little1, _rotation.Little1);
            SmoothMotion(_little2, _rotation.Little2);
            SmoothMotion(_little3, _rotation.Little3);
            SmoothMotion(_middle1, _rotation.Middle1);
            SmoothMotion(_middle2, _rotation.Middle2);
            SmoothMotion(_middle3, _rotation.Middle3);
            SmoothMotion(_ring1, _rotation.Ring1);
            SmoothMotion(_ring2, _rotation.Ring2);
            SmoothMotion(_ring3, _rotation.Ring3);
            SmoothMotion(_thumb1, _rotation.Thumb1);
            SmoothMotion(_thumb2, _rotation.Thumb2);
            SmoothMotion(_thumb3, _rotation.Thumb3);
        }

        
        private void SmoothMotion(Transform hum, Quaternion rotation)
        {
            // float easedT = EaseOutSine(_t);
            hum.rotation = Quaternion.Slerp(hum.rotation, rotation, _t);
        }
        
        public static float EaseOutSine(float x) => Mathf.Sin((x * Mathf.PI) / 2f);
        
        
        private static float EaseOutQuint(float x) => 1f - Mathf.Pow(1f - x, 5f);
    }
}