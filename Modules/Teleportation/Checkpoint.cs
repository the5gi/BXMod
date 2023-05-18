﻿using GorillaLocomotion;
using Bark.Extensions;
using Bark.Gestures;
using Bark.GUI;
using Bark.Patches;
using Bark.Tools;
using Bark.Modules.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bark.Modules.Teleportation
{
    public class Checkpoint : BarkModule
    {
        public static Checkpoint Instance;

        private Transform checkpointMarker;
        private LineRenderer bananaLine;
        private Vector3 checkpointPosition, checkpointMarkerPosition;
        private float checkpointRotation;
        private bool pointSet;
        GameObject checkpointPrefab, bananaLinePrefab;

        void Awake() { Instance = this; }

        protected override void Start()
        {
            try
            {
                base.Start();
                checkpointPrefab = Plugin.assetBundle.LoadAsset<GameObject>("Checkpoint Banana");
                checkpointPrefab.gameObject.SetActive(false);
                bananaLinePrefab = Plugin.assetBundle.LoadAsset<GameObject>("Banana Line");
                bananaLinePrefab.gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                Logging.LogException(e);
            }
        }

        void LeftTriggered()
        {
            if (enabled)
                StartCoroutine(GrowBananas());
        }

        void RightTriggered()
        {
            if (enabled && pointSet)
                StartCoroutine(GoBananas());
        }

        // Creates the checkpoint
        IEnumerator GrowBananas()
        {
            checkpointMarker.gameObject.SetActive(true);
            float startTime = Time.time;
            while (GestureTracker.Instance.leftTriggered)
            {
                float scale = Mathf.Lerp(0, Player.Instance.scale, (Time.time - startTime) / 2f);
                checkpointMarker.position = Player.Instance.leftHandTransform.position + Vector3.up * .15f * Player.Instance.scale;
                checkpointMarker.localScale = Vector3.one * scale;
                if (Mathf.Abs(scale - Player.Instance.scale) < .01f)
                {
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(UnityEngine.Random.Range(40, 56), false, 0.1f);
                    GestureTracker.Instance.HapticPulse(true);
                    checkpointPosition = Player.Instance.bodyCollider.transform.position;
                    checkpointRotation = Player.Instance.headCollider.transform.eulerAngles.y;
                    pointSet = true;
                    checkpointMarker.localScale = Vector3.one * Player.Instance.scale;
                    checkpointMarkerPosition = checkpointMarker.position;
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
            if (!pointSet)
            {
                checkpointMarker.localScale = Vector3.zero;
                checkpointMarker.gameObject.SetActive(pointSet);
            }
            else
            {
                checkpointMarker.position = checkpointMarkerPosition;
                checkpointMarker.localScale = Vector3.one * Player.Instance.scale;
            }
        }

        // Warps the player to the checkpoint
        IEnumerator GoBananas()
        {
            bananaLine.gameObject.SetActive(true);
            float startTime = Time.time;
            Vector3 startPos, endPos;
            while (GestureTracker.Instance.rightTriggered && pointSet)
            {
                startPos = Player.Instance.rightHandTransform.position;
                bananaLine.SetPosition(1, startPos);
                endPos = Vector3.Lerp(startPos, checkpointMarker.transform.position, (Time.time - startTime) / 2f);
                bananaLine.SetPosition(0, endPos);
                bananaLine.startWidth = bananaLine.endWidth = Player.Instance.scale * .1f;
                bananaLine.material.mainTextureScale = new Vector2(Player.Instance.scale, 1);
                if (Vector3.Distance(endPos, checkpointMarker.transform.position) < .01f)
                {
                    TeleportPatch.TeleportPlayer(checkpointPosition, checkpointRotation);
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
            bananaLine.gameObject.SetActive(false);
        }

        void FixedUpdate()
        {
            checkpointMarker.Rotate(Vector3.up, 90 * Time.fixedDeltaTime, Space.World);
        }

        List<GorillaTriggerBox> markedTriggers;
        protected override void OnEnable()
        {
            try
            {
                if (!MenuController.Instance.Built) return;
                base.OnEnable();
                checkpointMarker = Instantiate(checkpointPrefab).transform;
                checkpointMarker.position = checkpointPosition;
                checkpointMarker.gameObject.SetActive(pointSet);
                bananaLine = Instantiate(bananaLinePrefab).GetComponent<LineRenderer>();
                markedTriggers = new List<GorillaTriggerBox>();
                foreach (var triggerBox in FindObjectsOfType<GorillaTriggerBox>())
                {
                    if (triggerBox?.gameObject?.GetComponent<CollisionObserver>()) continue;
                    var observer = triggerBox.gameObject.AddComponent<CollisionObserver>();
                    // Sometimes you just can't add a collision observer for some reason. If this happens, give up.
                    if (!triggerBox?.gameObject?.GetComponent<CollisionObserver>()) continue;

                    observer.OnTriggerStayed += (box, collider) =>
                    {
                        if (collider == Player.Instance.bodyCollider)
                            ClearCheckpoint();
                    };
                    markedTriggers.Add(triggerBox);
                }
                GestureTracker.Instance.OnLeftTriggerPressed += LeftTriggered;
                GestureTracker.Instance.OnRightTriggerPressed += RightTriggered;
            }
            catch (Exception e) { Logging.LogException(e); }
        }


        public void ClearCheckpoint()
        {
            if (!pointSet) return;
            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(68, false, 1f);
            checkpointMarker.gameObject.SetActive(false);
            pointSet = false;
            bananaLine.gameObject.SetActive(false);
        }

        protected override void Cleanup()
        {
            bananaLine?.gameObject.Obliterate();
            checkpointMarker?.gameObject.Obliterate();
            foreach (var triggerBox in markedTriggers)
            {
                triggerBox.GetComponent<CollisionObserver>()?.Obliterate();
            }
            GestureTracker.Instance.OnLeftTriggerPressed -= LeftTriggered;
            GestureTracker.Instance.OnRightTriggerPressed -= RightTriggered;
        }

        public override string DisplayName()
        {
            return "Checkpoint";
        }

        public override string Tutorial()
        {
            return "Hold [Left Trigger] to spawn a checkpoint. Hold [Right Trigger] to return to it.";
        }
    }
}
