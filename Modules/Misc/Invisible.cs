using Bark.GUI;
using UnityEngine;
using UnityEngine.XR;

namespace Bark.Modules.Misc
{
    internal class Invisible : BarkModule
    {
        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }


        void FixedUpdate()
        {
            if (this.enabled == true)
            {
                InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                bool rightHandTrigger = false;
                bool tempState = rightController.TryGetFeatureValue(CommonUsages.triggerButton, out rightHandTrigger);
                if (rightHandTrigger)
                {
                    GorillaTagger.Instance.myVRRig.transform.position = new Vector3(1050f, 1050f, 1050f);
                    GorillaTagger.Instance.myVRRig.enabled = false;
                }
                else
                {
                    GorillaTagger.Instance.myVRRig.enabled = true;
                }
            }
        }

        public override string DisplayName()
        {
            return "Invisible";
        }

        public override string Tutorial()
        {
            return "right trigger go invisible retarded";
        }

        protected override void Cleanup()
        {
            return;
        }
    }
}
