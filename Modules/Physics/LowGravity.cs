using Bark.GUI;
using UnityEngine;

namespace Bark.Modules.Physics
{
    public class LowGravity : BarkModule
    {

        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
            this.enabled = false;
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
