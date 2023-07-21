using BXMod.GUI;
using GorillaLocomotion;

namespace BXMod.Modules
{
    public class Speed : BXModule
    {
        public static float baseVelocityLimit, scale;
        public float _scale = 1.25f;
        public static bool active = false;

        void FixedUpdate()
        {
            if (this.enabled)
            {
                Player.Instance.jumpMultiplier = 1.3f * _scale;
                Player.Instance.maxJumpSpeed = 8.5f * _scale;
            }
        }

        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
            scale = _scale;
            baseVelocityLimit =  Player.Instance.velocityLimit;
            Player.Instance.velocityLimit = baseVelocityLimit * scale;
        }

        protected override void Cleanup()
        {
            return;
        }

        public override string DisplayName()
        {
            return "Speed Boost";
        }

        public override string Tutorial()
        {
            return "Effect: Increases your jump strength.";
        }

    }
}
