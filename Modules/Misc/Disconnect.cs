using Photon.Pun;
using BXMod.GUI;

namespace BXMod.Modules.Misc
{
    public class Disconnect : BXModule
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
