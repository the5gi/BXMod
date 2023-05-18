using Bark.GUI;
using GorillaLocomotion;
using UnityEngine;

namespace Bark.Modules.Physics
{
    public class Freeze : BarkModule
    {
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
            return "REMOVED";
        }

        public override string Tutorial()
        {
            return "REMOVED";
        }

    }
}
