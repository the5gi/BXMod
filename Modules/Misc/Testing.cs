using System;
using System.Linq;
using Bark.Tools;
using BXMod.Modules;
using BXMod.Tools;
using Photon.Voice.Unity;
using UnityEngine;
using Player = GorillaLocomotion.Player;

namespace BXMod.Modules.Misc
{
    public class Testing : BXModule
    {
        public override string DisplayName()
        {
            return "testing";
        }

        public override string Tutorial()
        {
            return "cool testing";
        }

        protected override void Cleanup()
        {
            return;
        }

        private void LateUpdate()
        {
            /*log("DEBUG1020 BEFORE RIGS");
            VRRig rig = Plugin.getLocalRig();
            Player player = Plugin.getLocalPlayer();
            log("-------- DEBUG1020START");
            if (rig == null) log("rig is null");
            for (int i = 0; i < GorillaParent.instance.vrrigs.Count(); i++)
            {
                VRRig otherRig = GorillaParent.instance.vrrigs[i];
                String myRigCoord = rig.leftHandTransform.position.x + " " +
                                    rig.leftHandTransform.position.y + " " +
                                    rig.leftHandTransform.position.z;
                String otherRigCoord = otherRig.leftHandTransform.position.x + " " +
                                       otherRig.leftHandTransform.position.y + " " +
                                       otherRig.leftHandTransform.position.z;
                log("check if lefthandsame? : myrig: " + myRigCoord + " | otherrig: " + otherRigCoord + " | final check = " + Plugin.isMyRig(otherRig));
            }
            log("-------- DEBUG1020END");*/
        }

        private static void log(String text)
        {
            Logging.logger.LogMessage(text);
        }
    }
}