using GorillaLocomotion;
using System;
using System.Collections.Generic;
using BXMod.Extensions;
using BXMod.Gestures;
using BXMod.GUI;
using BXMod.Tools;
using UnityEngine;

namespace BXMod.Modules.Multiplayer
{

    public class BoxingMarker : MonoBehaviour { }

    public class Boxing : BXModule
    {
        public float forceMultiplier = 50;
        private PunchTracker tracker;
        private Collider punchCollider;
        private List<GameObject> gloves = new List<GameObject>();
        private List<BoxingMarker> markers = new List<BoxingMarker>();

        private class PunchTracker
        {
            public Collider collider;
            public Vector3 lastPos;
        }

        void CreateGloves()
        {

            foreach (var rig in GorillaParent.instance.vrrigs)
            {
                try
                {
                    if (Plugin.isMyRig(rig) ||
                        rig.gameObject.GetComponent<BoxingMarker>()) continue;

                    markers.Add(rig.gameObject.AddComponent<BoxingMarker>());
                    gloves.Add(CreateGlove(rig.leftHandTransform));
                    gloves.Add(CreateGlove(rig.rightHandTransform, false));
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                }
            }

        }

        private GameObject CreateGlove(Transform parent, bool isLeft = true)
        {
            var glove = Instantiate(Plugin.assetBundle.LoadAsset<GameObject>("Boxing Glove"), parent, false);
            string side = isLeft ? "Left" : "Right";
            glove.name = $"Boxing Glove ({side})";
            float x = isLeft ? 1 : -1;
            glove.transform.localScale = new Vector3(x, 1, 1);
            glove.layer = BXModInteractor.InteractionLayer;
            foreach (Transform child in glove.transform)
                child.gameObject.layer = BXModInteractor.InteractionLayer;
            return glove;
        }

        void FixedUpdate()
        {
            if (Time.frameCount % 300 == 0) CreateGloves();
        }

        private void DoPunch()
        {
            Vector3 force = (tracker.collider.transform.position - tracker.lastPos);
            if (force.magnitude > 1)
                force.Normalize();
            force *= forceMultiplier;
            Player.Instance.bodyCollider.attachedRigidbody.velocity += force;
            tracker = null;
        }

        private void RegisterTracker(Collider collider)
        {
            tracker = new PunchTracker
            {
                collider = collider,
                lastPos = collider.transform.position
            };
        }

        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
            try
            {
                ReloadConfiguration();
                var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                capsule.name = "BarkPunchDetector";
                capsule.transform.SetParent(Player.Instance.bodyCollider.transform, false);
                capsule.layer = BXModInteractor.InteractionLayer;
                capsule.GetComponent<MeshRenderer>().enabled = false;

                punchCollider = capsule.GetComponent<Collider>();
                punchCollider.isTrigger = true;
                punchCollider.transform.localScale = new Vector3(.5f, .35f, .5f);
                punchCollider.transform.localPosition += new Vector3(0, .3f, 0);

                var observer = capsule.AddComponent<CollisionObserver>();
                observer.OnTriggerEntered += (obj, collider) =>
                {
                    if (collider.name != "MM Glove" || collider == tracker?.collider) return;
                    RegisterTracker(collider);
                    Invoke(nameof(DoPunch), 0.1f);
                };

                CreateGloves();
            }
            catch (Exception e)
            {
                Logging.Exception(e);
            }
        }

        protected override void Cleanup()
        {
            punchCollider?.gameObject?.Obliterate();
            foreach (GameObject g in gloves)
            {
                g?.Obliterate();
            }
            foreach (BoxingMarker m in markers)
            {
                m?.Obliterate();
            }
        }

        protected override void ReloadConfiguration()
        {
            forceMultiplier = (50f);
        }

        public override string DisplayName()
        {
            return "Boxing";
        }

        public override string Tutorial()
        {
            return "Effect: Other players can punch you around.";
        }
    }
}