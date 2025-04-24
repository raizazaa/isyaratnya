using UnityEngine;

namespace Sign
{
    public class BodyRotation
    {
        private Quaternion _spine = Quaternion.identity;
        private Quaternion _chest = Quaternion.identity;
        private Quaternion _upperChest = Quaternion.identity;
        private Quaternion _neck = Quaternion.identity;
        private Quaternion _head = Quaternion.identity;
        
        public Quaternion Spine => _spine;
        
        

        public void UpdateRotation(FrameData rData, FrameData lData)
        {
            var upperArmR = rData.ConvertToVector3(rData.u);
            var upperArmL = lData.ConvertToVector3(lData.u);
            
            var normal = Vector3.Cross(upperArmR, upperArmL).normalized;
            
            _spine = Quaternion.LookRotation(normal, Vector3.up);
        }
        
        

    }
}