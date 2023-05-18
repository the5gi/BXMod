using Bark.Modules;
using UnityEngine;

namespace Bark.GUI
{
    internal class FirstPersonCamera : BarkModule
    {
        GameObject sCamera = GameObject.Find("Shoulder Camera");

        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
            sCamera.SetActive(false);
        }

        protected override void OnDisable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnDisable();
            sCamera.SetActive(true);
        }

        public override string DisplayName()
        {
            return "First Person Camera";
        }

        public override string Tutorial()
        {
            return "you fucking idiot, just toggle it you nunce";
        }

        protected override void Cleanup()
        {
            return;
        }
    }
}
