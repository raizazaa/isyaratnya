using System;
using UniHumanoid;
using UnityEngine;

namespace Sign
{
    public class SideController
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
        
        #region rest
        private Quaternion _upperArmRest;
        private Quaternion _lowerArmRest;
        private Quaternion _handRest;
        private Quaternion _index1Rest;
        private Quaternion _index2Rest;
        private Quaternion _index3Rest;
        private Quaternion _little1Rest;
        private Quaternion _little2Rest;
        private Quaternion _little3Rest;
        private Quaternion _middle1Rest;
        private Quaternion _middle2Rest;
        private Quaternion _middle3Rest;
        private Quaternion _ring1Rest;
        private Quaternion _ring2Rest;
        private Quaternion _ring3Rest;
        private Quaternion _thumb1Rest;
        private Quaternion _thumb2Rest;
        private Quaternion _thumb3Rest;

        private SideRotation _rest;
        #endregion

        private void RestRotation(Side side)
        {
            switch (side)
            {
                case Side.Right:
                {
                    _rest = new SideRotation(
                        side,
                        new Quaternion(-0.00028563663363456726f, -0.18234360218048097f, -0.5197166800498962f,0.8346530795097351f),
                        new Quaternion(-0.23452681303024293f, -0.5657134652137756f, -0.6696068644523621f, 0.42022907733917239f),
                        new Quaternion(-0.24252012372016908f, -0.5651054978370667f, -0.6650325655937195f, 0.4237504005432129f),
                        new Quaternion(-0.01929059624671936f, -0.569776713848114f, -0.7876396179199219f, 0.23368030786514283f),
                        new Quaternion(0.30970826745033266f, -0.04105878248810768f, -0.9358726739883423f, 0.16290408372879029f),
                        new Quaternion(-0.34970054030418398f, -0.38750192523002627f, 0.8084422945976257f, -0.2719816565513611f),
                        new Quaternion(0.11608579009771347f, -0.5014656782150269f, -0.8475505709648132f, 0.12928691506385804f),
                        new Quaternion(0.28739407658576968f, -0.19172079861164094f, -0.9377877712249756f, 0.0346752405166626f),
                        new Quaternion(-0.34970054030418398f, -0.38750192523002627f, 0.8084422945976257f, -0.2719816565513611f),
                        new Quaternion(-0.01929059624671936f, -0.569776713848114f, -0.7876396179199219f, 0.23368030786514283f),
                        new Quaternion(0.30970826745033266f, -0.04105878248810768f, -0.9358726739883423f, 0.16290408372879029f),
                        new Quaternion(-0.34970054030418398f, -0.38750192523002627f, 0.8084422945976257f, -0.2719816565513611f),
                        new Quaternion(-0.01929059624671936f, -0.569776713848114f, -0.7876396179199219f, 0.23368030786514283f),
                        new Quaternion(0.30970826745033266f, -0.04105878248810768f, -0.9358726739883423f, 0.16290408372879029f),
                        new Quaternion(-0.34970054030418398f, -0.38750192523002627f, 0.8084422945976257f, -0.2719816565513611f),
                        new Quaternion(-0.016374733299016954f, -0.950526773929596f, -0.00016364455223083496f, 0.3102114796638489f),
                        new Quaternion(0.007853832095861435f, -0.9272381067276001f, -0.01618596911430359f, 0.3740409016609192f),
                        new Quaternion(-0.0738430842757225f, -0.9250386953353882f, 0.03450988978147507f, 0.3710256814956665f)
                        );

                } break;
                case Side.Left:
                {
                    _rest = new SideRotation(
                        side, 
                        new Quaternion(-0.002794545143842697f, 0.13428565859794618f, 0.5385918617248535f, 0.8317924737930298f),
                        new Quaternion(-0.16053900122642518f, 0.6135743856430054f, 0.617160975933075f, 0.46568888425827029f),
                        new Quaternion(-0.3477201461791992f, 0.6581090688705444f, 0.4798305630683899f, 0.46448495984077456f),
                        new Quaternion(-0.26218199729919436f, 0.8244059681892395f, 0.45652157068252566f, 0.2078559249639511f),
                        new Quaternion(-0.04835878312587738f, -0.5498559474945068f, -0.7736608982086182f, -0.31107738614082339f),
                        new Quaternion(-0.2490319013595581f, 0.4286673069000244f, -0.6970047354698181f, -0.518085241317749f),
                        new Quaternion(-0.26218199729919436f, 0.8244059681892395f, 0.45652157068252566f, 0.2078559249639511f),
                        new Quaternion(-0.04835878312587738f, -0.5498559474945068f, -0.7736608982086182f, -0.31107738614082339f),
                        new Quaternion(-0.2490319013595581f, 0.4286673069000244f, -0.6970047354698181f, -0.518085241317749f),
                        new Quaternion(-0.26218199729919436f, 0.8244059681892395f, 0.45652157068252566f, 0.2078559249639511f),
                        new Quaternion(-0.04835878312587738f, -0.5498559474945068f, -0.7736608982086182f, -0.31107738614082339f),
                        new Quaternion(-0.2490319013595581f, 0.4286673069000244f, -0.6970047354698181f, -0.518085241317749f),
                        new Quaternion(-0.26218199729919436f, 0.8244059681892395f, 0.45652157068252566f, 0.2078559249639511f),
                        new Quaternion(-0.04835878312587738f, -0.5498559474945068f, -0.7736608982086182f, -0.31107738614082339f),
                        new Quaternion(-0.2490319013595581f, 0.4286673069000244f, -0.6970047354698181f, -0.518085241317749f),
                        new Quaternion(0.05369441211223602f, 0.9394199252128601f, -0.17361965775489808f, 0.2906261384487152f),
                        new Quaternion(0.04570780694484711f, 0.9268407821655273f, -0.176666259765625f, 0.3281267285346985f),
                        new Quaternion(0.1508832424879074f, 0.8979522585868836f, -0.060395438224077228f, 0.4089851379394531f)
                    );
                } break;
            } 
        }
        
        public SideController(Humanoid humanoid, Side side)
        {
            Populate(humanoid, side);
            RestRotation(side);
        }
        private void Populate(Humanoid humanoid, Side side)
        {
            switch (side)
            {
                case Side.Right:
                {
                    _upperArm = humanoid.RightUpperArm;
                    _lowerArm = humanoid.RightLowerArm;
                    _hand = humanoid.RightHand;
                    _index1 = humanoid.RightIndexProximal;
                    _index2 = humanoid.RightIndexIntermediate;
                    _index3 = humanoid.RightIndexDistal;
                    _little1 = humanoid.RightLittleProximal;
                    _little2 = humanoid.RightLittleIntermediate;
                    _little3 = humanoid.RightLittleDistal;
                    _middle1 = humanoid.RightMiddleProximal;
                    _middle2 = humanoid.RightMiddleIntermediate;
                    _middle3 = humanoid.RightMiddleDistal;
                    _ring1 = humanoid.RightRingProximal;
                    _ring2 = humanoid.RightRingIntermediate;
                    _ring3 = humanoid.RightRingDistal;
                    _thumb1 = humanoid.RightThumbProximal;
                    _thumb2 = humanoid.RightThumbIntermediate;
                    _thumb3 = humanoid.RightThumbDistal;
                } break;
                case Side.Left:
                {
                    _upperArm = humanoid.LeftUpperArm;
                    _lowerArm = humanoid.LeftLowerArm;
                    _hand = humanoid.LeftHand;
                    _index1 = humanoid.LeftIndexProximal;
                    _index2 = humanoid.LeftIndexIntermediate;
                    _index3 = humanoid.LeftIndexDistal;
                    _little1 = humanoid.LeftLittleProximal;
                    _little2 = humanoid.LeftLittleIntermediate;
                    _little3 = humanoid.LeftLittleDistal;
                    _middle1 = humanoid.LeftMiddleProximal;
                    _middle2 = humanoid.LeftMiddleIntermediate;
                    _middle3 = humanoid.LeftMiddleDistal;
                    _ring1 = humanoid.LeftRingProximal;
                    _ring2 = humanoid.LeftRingIntermediate;
                    _ring3 = humanoid.LeftRingDistal;
                    _thumb1 = humanoid.LeftThumbProximal;
                    _thumb2 = humanoid.LeftThumbIntermediate;
                    _thumb3 = humanoid.LeftThumbDistal;
                } break;
            } 
            
        }
        private SideRotation _rotation;
        private float _t;
        public void Update(SideRotation rotation, float t, bool inGesture)
        {
            _rotation = inGesture ? rotation : _rest;
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
            hum.rotation = Quaternion.Slerp(hum.rotation, rotation, _t);
        }
    }
}