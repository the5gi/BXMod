using Bark.Extensions;
using Bark.Gestures;
using Bark.GUI;
using Bark.Tools;
using Photon.Pun;
using System;
using UnityEngine;

namespace Bark.Modules.Misc
{
    public class Disconnect : BarkModule
    {
        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.Disconnect();
            }
            this.enabled = false;
        }


        public override string DisplayName()
        {
            return "Disconnect";
        }

        public override string Tutorial()
        {
            return "Disconnects you from lobby bozo, stink fart cheese";
        }

        protected override void Cleanup()
        {
            return;
        }
    }
}
