using System.Collections.Generic;
using UnityEngine;

namespace Sign {
    [System.Serializable]
    public class FrameData
    {
        #region data
        public float[] u;
        public float[] l;
        public float[] h;
        public float[] t1;
        public float[] t2;
        public float[] t3;
        public float[] t4;
        public float[] i1;
        public float[] i2;
        public float[] i3;
        public float[] i4;
        public float[] m1;
        public float[] m2;
        public float[] m3;
        public float[] m4;
        public float[] r1;
        public float[] r2;
        public float[] r3;
        public float[] r4;
        public float[] p1;
        public float[] p2;
        public float[] p3;
        public float[] p4;
        #endregion
        
        private Side _side;
        private Quaternion _bodyRotation;
        private Vector3 _bodyPosition;

        public void SetBodyRotation(Quaternion q) => _bodyRotation = q;
        public void SetBodyPosition(Vector3 v) => _bodyPosition = v;

        public void PrintData()
        {
            Debug.Log(u);
            Debug.Log(l);
            Debug.Log(h);
            Debug.Log(t1);
            Debug.Log(t2);
            Debug.Log(t3);
            Debug.Log(t4);
            Debug.Log(i1);
            Debug.Log(i2);
            Debug.Log(i3);
            Debug.Log(i4);
            Debug.Log(m1);
            Debug.Log(m2);
            Debug.Log(m3);
            Debug.Log(m4);
            Debug.Log(r1);
            Debug.Log(r2);
            Debug.Log(r3);
            Debug.Log(r4);
            Debug.Log(p1);
            Debug.Log(p2);
            Debug.Log(p3);
            Debug.Log(p4);
        }

        public Vector3 ConvertToVector3(float[] data)
        {
            return data != null ? new Vector3(data[0], data[1], data[2]) : Vector3.zero;
        }

        public Side SetSide(Side side) => _side = side;

        private Vector3 GetLowerArmFacing()
        {
            var a = ConvertToVector3(l); // lowerarm
            var b = ConvertToVector3(h); // hand
            var c = ConvertToVector3(m1); // middle1

            var ab = b - a;
            var ac = c - a;
            
            var palm = a + Vector3.Dot(ab, ac) / Vector3.Dot(ac, ac) * ac - b;
            
            return c != Vector3.zero ? palm.normalized : Vector3.zero;
        }

        private Vector3 GetHandFacing()
        {
            var a = ConvertToVector3(i1) - ConvertToVector3(h);
            var b = ConvertToVector3(p1) - ConvertToVector3(h);

            var palm = -Vector3.Cross(a, b).normalized;
            
            return BoolSide() ? palm : -palm;
        }
        
        private bool BoolSide() => _side == Side.Right;

        public bool CheckHand()
        {
            return m1 != null;
        }

        private Vector3 GetLocalPosition(float[] origin, float[] target)
        {
            var vTarget = ConvertToVector3(target);
            var vOrigin = ConvertToVector3(origin);
            var local = vTarget - vOrigin;

            return (vOrigin + _bodyRotation * local).normalized;

        }
        
        #region directions
        public Vector3 UpperArm() => (ConvertToVector3(l) - ConvertToVector3(u)).normalized;
        public Vector3 LowerArm() => (ConvertToVector3(h) - ConvertToVector3(l)).normalized;
        public Vector3 LowerArmFacing() => GetLowerArmFacing();

        public Vector3 Hand() => CheckHand() ? (ConvertToVector3(m1) - ConvertToVector3(h)).normalized : Vector3.zero;
        public Vector3 HandFacing() => CheckHand() ? GetHandFacing() : Vector3.zero;
        
        public Vector3 ThumbFacing1() => (ConvertToVector3(i1) - ConvertToVector3(t1)).normalized;
        public Vector3 ThumbFacing2() => (ConvertToVector3(i1) - ConvertToVector3(t2)).normalized;
        public Vector3 ThumbFacing3() => (ConvertToVector3(i1) - ConvertToVector3(t3)).normalized;
        public Vector3 Thumb1() => (ConvertToVector3(t2) - ConvertToVector3(t1)).normalized;
        public Vector3 Thumb2() => (ConvertToVector3(t3) - ConvertToVector3(t2)).normalized;
        public Vector3 Thumb3() => (ConvertToVector3(t4) - ConvertToVector3(t3)).normalized;
        
        public Vector3 Index1() => (ConvertToVector3(i2) - ConvertToVector3(i1)).normalized;
        public Vector3 Index2() => (ConvertToVector3(i3) - ConvertToVector3(i2)).normalized;
        public Vector3 Index3() => (ConvertToVector3(i4) - ConvertToVector3(i3)).normalized;
        
        public Vector3 Middle1() => (ConvertToVector3(m2) - ConvertToVector3(m1)).normalized;
        public Vector3 Middle2() => (ConvertToVector3(m3) - ConvertToVector3(m2)).normalized;
        public Vector3 Middle3() => (ConvertToVector3(m4) - ConvertToVector3(m3)).normalized;
        
        public Vector3 Ring1() => (ConvertToVector3(r2) - ConvertToVector3(r1)).normalized;
        public Vector3 Ring2() => (ConvertToVector3(r3) - ConvertToVector3(r2)).normalized;
        public Vector3 Ring3() => (ConvertToVector3(r4) - ConvertToVector3(r3)).normalized;
        
        public Vector3 Little1() => (ConvertToVector3(p2) - ConvertToVector3(p1)).normalized;
        public Vector3 Little2() => (ConvertToVector3(p3) - ConvertToVector3(p2)).normalized;
        public Vector3 Little3() => (ConvertToVector3(p4) - ConvertToVector3(p3)).normalized;
        #endregion

    }
}

