using GorillaLocomotion;
using Bark.Tools;
using UnityEngine;
using System.Reflection;
using Bark.Modules.Physics;
using Bark.GUI;
using UnityEngine.XR;

namespace Bark.Modules.Movement
{
    public class FlySpeedControl : BarkModule
    {

        private bool leftTriggerDown = false;
        private bool rightTriggerDown = false;

        void FixedUpdate()
        {
            if (this.enabled)
            {
                InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                bool rightTrigger;
                bool temp1 = rightController.TryGetFeatureValue(CommonUsages.triggerButton, out rightTrigger);
                InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                bool leftTrigger;
                bool temp2 = leftController.TryGetFeatureValue(CommonUsages.triggerButton, out leftTrigger);


                if (leftTrigger && rightTrigger)
                {
                    return;
                }

                if (leftTriggerDown && leftTrigger)
                {
                    return;
                }
                else if (leftTriggerDown && !leftTrigger)
                {
                    leftTriggerDown = false;
                }

                if (rightTriggerDown && rightTrigger)
                {
                    return;
                }
                else if (rightTriggerDown && !rightTrigger)
                {
                    rightTriggerDown = false;
                }

                if (rightTrigger)
                {
                    if (SuperMan.flySpeedIndex == 3)
                    {
                        return;
                    }
                    SuperMan.flySpeedIndex++;
                    rightTriggerDown = true;
                } else if (leftTrigger)
                {
                    if (SuperMan.flySpeedIndex == 0)
                    {
                        return;
                    }
                    SuperMan.flySpeedIndex--;
                    leftTriggerDown = true;
                }
            }
        }

        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
        }

        protected override void Cleanup()
        {
            return;
        }

        public override string DisplayName()
        {
            return "Fly Speed Control";
        }

        public override string Tutorial()
        {
            return "Use the left [decrease] and right [increase] to change the speed of flight. ";
        }

    }
}


