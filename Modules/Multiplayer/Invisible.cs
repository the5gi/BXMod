using BXMod.GUI;
using UnityEngine;
using UnityEngine.XR;

namespace BXMod.Modules.Misc
{
    internal class Invisible : BXModule
    {
        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
        }


        void FixedUpdate()
        {
            if (this.enabled)
            {
                InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                bool rightHandTrigger;
                rightController.TryGetFeatureValue(CommonUsages.triggerButton, out rightHandTrigger);
                if (rightHandTrigger)
                {
                    //getLocalRig().transform.position = new Vector3(1050f, 1050f, 1050f);
                    //Plugin.getLocalRig().enabled = false;
                    GorillaTagger.Instance.myVRRig.transform.position = new Vector3(1050f, 1050f, 1050f);
                    GorillaTagger.Instance.myVRRig.enabled = false;
                }
                else
                {
                    GorillaTagger.Instance.myVRRig.enabled = true;
                    //Plugin.getLocalRig().enabled = true;
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