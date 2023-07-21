using BXMod.GUI;

namespace BXMod.Modules.Physics
{
    public class Freeze : BXModule
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
