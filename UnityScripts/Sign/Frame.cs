using UnityEngine;

namespace Sign
{
    [System.Serializable]
    public class Frame
    {
        public FrameData r;
        public FrameData l;

        private Quaternion _bodyRotation;
        private Vector3 _bodyPosition;
        public Quaternion BodyRotation() => _bodyRotation;
        public Vector3 BodyPosition() => _bodyPosition;
        
        public void SetBody()
        {
            var right = r.ConvertToVector3(r.u);
            var left = r.ConvertToVector3(l.u);

            var shoulderMid = (right + left) * 0.5f;
            _bodyPosition = shoulderMid;
            
            var shoulderDir = right - left;
            var bodyRight = shoulderDir.normalized;
            
            var bodyUp = Vector3.Cross(shoulderDir, Vector3.up).normalized;

            var bodyForward = Vector3.Cross(bodyUp, bodyRight).normalized;
            _bodyRotation = Quaternion.LookRotation(bodyForward, bodyUp);
            
            SetFrame();
            
        }

        private void SetFrame()
        {
            r.SetBodyPosition(_bodyPosition);
            r.SetBodyRotation(_bodyRotation);
            l.SetBodyPosition(_bodyPosition);
            l.SetBodyRotation(_bodyRotation);
        }
    }
}