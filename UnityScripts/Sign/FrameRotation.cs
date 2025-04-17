using System;
using UnityEngine;

namespace Sign
{
    public class FrameRotation
    {
        #region quat
        private Quaternion _upperArm = Quaternion.identity;
        private Quaternion _lowerArm = Quaternion.identity;
        private Quaternion _hand = Quaternion.identity;
        private Quaternion _index1 = Quaternion.identity;
        private Quaternion _index2 = Quaternion.identity;
        private Quaternion _index3 = Quaternion.identity;
        private Quaternion _little1 = Quaternion.identity;
        private Quaternion _little2 = Quaternion.identity;
        private Quaternion _little3 = Quaternion.identity;
        private Quaternion _middle1 = Quaternion.identity;
        private Quaternion _middle2 = Quaternion.identity;
        private Quaternion _middle3 = Quaternion.identity;
        private Quaternion _ring1 = Quaternion.identity;
        private Quaternion _ring2 = Quaternion.identity;
        private Quaternion _ring3 = Quaternion.identity;
        private Quaternion _thumb1 = Quaternion.identity;
        private Quaternion _thumb2 = Quaternion.identity;
        private Quaternion _thumb3 = Quaternion.identity;
        #endregion
        
        #region getter
        public Quaternion UpperArm => _upperArm;
        public Quaternion LowerArm => _lowerArm;
        public Quaternion Hand => _hand;
        public Quaternion Index1 => _index1;
        public Quaternion Index2 => _index2;
        public Quaternion Index3 => _index3;
        public Quaternion Little1 => _little1;
        public Quaternion Little2 => _little2;
        public Quaternion Little3 => _little3;
        public Quaternion Middle1 => _middle1;
        public Quaternion Middle2 => _middle2;
        public Quaternion Middle3 => _middle3;
        public Quaternion Ring1 => _ring1;
        public Quaternion Ring2 => _ring2;
        public Quaternion Ring3 => _ring3;
        public Quaternion Thumb1 => _thumb1;
        public Quaternion Thumb2 => _thumb2;
        public Quaternion Thumb3 => _thumb3;
        #endregion
        
        
        private Vector3 _palmNormal = Vector3.zero;
        private Side _side;
        private FrameData _frameData;
        private bool _checkHand;
        public bool CheckHand => _checkHand;

        public FrameRotation(Side side)
        {
            _side = side;
        }

        public void UpdateRotation(FrameData frameData)
        {
            _frameData = frameData;
            _checkHand = _frameData.CheckHand();
            if (!_checkHand) return;
            ArmRotation();
            HandRotation();
            FingersRotation();
        }

        private void ArmRotation()
        {
            _upperArm = FromRotateTarget(_frameData.UpperArm());
            _lowerArm = LookRotateTarget(_frameData.LowerArm(), _frameData.HandFacing());
        }

        private void HandRotation()
        {
            _palmNormal = _frameData.HandFacing();
            _hand = LookRotateTarget(_frameData.Hand(), _frameData.HandFacing());
        }
        
        private void FingersRotation()
        {
            _index1 = LookRotateTarget(_frameData.Index1(), _frameData.HandFacing());
            _index2 = LookRotateTarget(_frameData.Index2(), _frameData.HandFacing());
            _index3 = LookRotateTarget(_frameData.Index3(), _frameData.HandFacing());
            
            _little1 = LookRotateTarget(_frameData.Little1(), _frameData.HandFacing());
            _little2 = LookRotateTarget(_frameData.Little2(), _frameData.HandFacing());
            _little3 = LookRotateTarget(_frameData.Little3(), _frameData.HandFacing());
            
            _middle1 = LookRotateTarget(_frameData.Middle1(), _frameData.HandFacing());
            _middle2 = LookRotateTarget(_frameData.Middle2(), _frameData.HandFacing());
            _middle3 = LookRotateTarget(_frameData.Middle3(), _frameData.HandFacing());
            
            _ring1 = LookRotateTarget(_frameData.Ring1(), _frameData.HandFacing());
            _ring2 = LookRotateTarget(_frameData.Ring2(), _frameData.HandFacing());
            _ring3 = LookRotateTarget(_frameData.Ring3(), _frameData.HandFacing());

            _thumb1 = LookRotateTarget(_frameData.Thumb1(), _frameData.ThumbFacing());
            _thumb2 = LookRotateTarget(_frameData.Thumb2(), _frameData.ThumbFacing());
            _thumb3 = LookRotateTarget(_frameData.Thumb3(), _frameData.ThumbFacing());
        }
        
        private Quaternion FromRotateTarget(Vector3 direction)
        {
            return Quaternion.FromToRotation(
                Quaternion.identity * (BoolSide() ? Vector3.right : Vector3.left), 
                direction
            );
        }
        
        private Quaternion LookRotateTarget(Vector3 direction, Vector3 facing)
        {
            var right = BoolSide() ? direction : -direction;
            var up = -facing;
            var forward = Vector3.Cross(right, up).normalized;
            Vector3.OrthoNormalize(ref right, ref up, ref forward);
            
            return Quaternion.LookRotation(forward, up);
        }
        
        private bool BoolSide()
        {
            return _side == Side.Right;
        }
        
        public void SetSide(Side side) => _side = side;
        
        public Quaternion[] GetFrameRotations => Rotations();

        private Quaternion[] Rotations()
        {
            return new Quaternion[]
            {
                _upperArm,
                _lowerArm,
                _hand,
                _index1,
                _index2,
                _index3,
                _little1,
                _little2,
                _little3,
                _middle1,
                _middle2,
                _middle3,
                _ring1,
                _ring2,
                _ring3,
                _thumb1,
                _thumb2,
                _thumb3,
            };
        }
        
        public void SetRotation(int index, Quaternion rotation)
        {
            switch (index)
            {
                case 0: _upperArm = rotation; break;
                case 1: _lowerArm = rotation; break;
                case 2: _hand = rotation; break;
                case 3: _index1 = rotation; break;
                case 4: _index2 = rotation; break;
                case 5: _index3 = rotation; break;
                case 6: _little1 = rotation; break;
                case 7: _little2 = rotation; break;
                case 8: _little3 = rotation; break;
                case 9: _middle1 = rotation; break;
                case 10: _middle2 = rotation; break;
                case 11: _middle3 = rotation; break;
                case 12: _ring1 = rotation; break;
                case 13: _ring2 = rotation; break;
                case 14: _ring3 = rotation; break;
                case 15: _thumb1 = rotation; break;
                case 16: _thumb2 = rotation; break;
                case 17: _thumb3 = rotation; break;
                
                default: throw new IndexOutOfRangeException("Invalid rotation index");
            }
        }
        public Quaternion GetRotation(int index)
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
        
    }
}