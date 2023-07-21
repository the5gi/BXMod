using BXMod;
using BXMod.Modules;

namespace BXMod.Modules.Movement
{
    internal class WaterBoost : BXModule
    {
        public override string DisplayName()
        {
            return "Water Boost";
        }

        public override string Tutorial()
        {
            return "WaterBooster, go faster in water.";
        }

        protected override void Cleanup()
        {
            
        }


        private float initialMaxJumpValue;
        private float initialSwimValueX;
        private float initialSwimValueY;
        private float boost = 2;

        private new void Start()
        {
            initialMaxJumpValue = Plugin.getLocalPlayer().maxJumpSpeed;
            initialSwimValueX = Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.x;
            initialSwimValueY = Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.y;
        }

        void LateUpdate()
        {
            if (enabled)
            {
                if (Plugin.getLocalPlayer().InWater)
                {
                    if (Plugin.getLocalPlayer().maxJumpSpeed != initialMaxJumpValue * boost)
                        Plugin.getLocalPlayer().maxJumpSpeed *= boost;
                    if (Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.x == initialSwimValueX &&
                        Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.y == initialSwimValueY)
                        Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.y *= boost;
                        Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.x =
                            Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.y;
                }
                else if (!Plugin.getLocalPlayer().InWater)
                {
                    if (Plugin.getLocalPlayer().maxJumpSpeed == initialMaxJumpValue * boost)
                        Plugin.getLocalPlayer().maxJumpSpeed = initialMaxJumpValue;
                    if (Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.x != initialSwimValueX &&
                        Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.y != initialSwimValueY)
                        Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.y = initialSwimValueY;
                        Plugin.getLocalPlayer().swimmingParams.handSpeedToRedirectAmountMinMax.x = initialSwimValueX;
                }
            }
        }
    }
}
