using UnityEngine;
using UniHumanoid;

namespace Sign
{
    public class HumanoidController
    {
        private BodyController _bodyController;
        private SideController _rSideController;
        private SideController _lSideController;

        public HumanoidController(BodyController bodyController, SideController rSideController, SideController lSideController)
        {
            _bodyController = bodyController; 
            _rSideController= rSideController;
            _lSideController = lSideController;
        }

        public void Update(BodyRotation bodyRotation, SideRotation rSideRotation, SideRotation lSideRotation, float t, bool inGesture)
        {
            _bodyController.Update(bodyRotation, t, inGesture);
            _rSideController.Update(rSideRotation, t, inGesture);
            _lSideController.Update(lSideRotation, t, inGesture);
        }
        
    }
}