using BXMod.GUI;
using UnityEngine;
using UnityEngine.XR;

namespace BXMod.Modules.Physics
{
    public class NoCollide : BXModule
    {
        private bool leftTriggerDown = false;
        private bool noClipOn = false;
        void FixedUpdate()
        { 
            if (this.enabled)
            {
                InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                bool leftTrigger;
                bool temp2 = leftController.TryGetFeatureValue(CommonUsages.triggerButton, out leftTrigger);
                if (leftTriggerDown && leftTrigger) return;
                if (leftTriggerDown && !leftTrigger) leftTriggerDown = false;

                if (leftTrigger && !noClipOn)
                {
                    foreach (MeshCollider meshCollider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    {
                        meshCollider.transform.localScale = meshCollider.transform.localScale / 10000f;
                    }
                    leftTriggerDown = true;
                    noClipOn = true;
                }
                else if (!leftTrigger && noClipOn)
                {
                    foreach (MeshCollider meshCollider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    {
                        meshCollider.transform.localScale = meshCollider.transform.localScale * 10000f;
                    }
                    noClipOn = false;
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
            //StartCoroutine(CleanupRoutine());
        } 

        /*IEnumerator CleanupRoutine()
        {
            Logging.LogDebug("Cleaning up noclip");

            if (!active) yield break;
            Player.Instance.locomotionEnabledLayers = baseMask;
            Player.Instance.bodyCollider.isTrigger = baseBodyIsTrigger;
            Player.Instance.headCollider.isTrigger = baseHeadIsTrigger;
            TeleportPatch.TeleportPlayer(activationLocation, activationAngle);
            active = false;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            TriggerBoxPatches.triggersEnabled = true;
            Logging.LogDebug("Enabling triggers");
        }*/

        public override string DisplayName()
        {
            return "NoClip";
        }

        public override string Tutorial()
        {
            return "Effect: Disables collisions. Automatically enables platforms.";
        }

    }
}
