
using System;
using System.Collections.Generic;
using BXMod.Extensions;
using BXMod.GUI;
using BXMod.Tools;
using UnityEngine;
using UnityEngine.XR;

namespace BXMod.Modules.Multiplayer
{

    public class XRay : BXModule
    {
        public static readonly string displayName = "ESP";
        List<XRayMarker> markers;
        void ApplyMaterial()
        {
            foreach (var rig in GorillaParent.instance.vrrigs)
            {
                if (Plugin.isMyRig(rig)) continue;
                try
                {
                    var marker = rig.gameObject.GetComponent<XRayMarker>();
                    if (marker) marker.Update();
                    else
                    {
                        markers.Add(rig.gameObject.AddComponent<XRayMarker>());
                    }
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                    Logging.Debug("rig is null:", rig is null);
                }
            }
        }
        void FixedUpdate()
        {
            if (Time.frameCount % 300 == 0) ApplyMaterial();
        }
        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
            markers = new List<XRayMarker>();
            ApplyMaterial();
        }

        protected override void Cleanup()
        {
            if (!MenuController.Instance.Built) return;
            foreach (var marker in markers)
                marker?.Obliterate();
        }

        public override string DisplayName()
        {
            return displayName;
        }
        public override string Tutorial()
        {
            return "Effect: Allows you to see other players through walls.";
        }

    }

    public class XRayMarker : MonoBehaviour
    {
        Material baseMaterial, material;
        VRRig rig;
        void Start()
        {
            rig = GetComponent<VRRig>();
            if (Plugin.isMyRig(rig)) this.Obliterate();
            material = Instantiate(Plugin.assetBundle.LoadAsset<Material>("X-Ray Material"));
            Update();
        }

        public void Update()
        {
            InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            bool leftPrimary;
            bool temp2 = leftController.TryGetFeatureValue(CommonUsages.primaryButton, out leftPrimary);
            if (rig.mainSkin.material.name.Contains("X-Ray") && leftPrimary)
            {
                rig.mainSkin.material = baseMaterial;
                return;
            }

            if (!rig.mainSkin.material.name.Contains("X-Ray"))
            {
                baseMaterial = rig.mainSkin.material;

                if (rig.mainSkin.material.name.Contains("infected"))
                {
                    material.color = new Color(0.18039215686f, 0.03529411764f, 0.03529411764f);
                }
                else
                {
                    material.color = baseMaterial.color;
                }
                material.mainTexture = baseMaterial.mainTexture;
                material.SetTexture("_MainTex", baseMaterial.mainTexture);
                rig.mainSkin.material = material;
            }
        }

        void OnDestroy()
        {
            rig.mainSkin.material = baseMaterial;
            Logging.Debug($"Reset material to {baseMaterial.name}");
        }
    }
}