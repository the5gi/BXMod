using GorillaLocomotion;
using Bark.GUI;
using UnityEngine;
using UnityEngine.XR;
using Bark.Modules.Physics;

namespace Bark.Modules.Movement
{
    public class SuperMan : BarkModule
    {
        /*float speedScale = 25f, acceleration = .2f;

        protected override void Start()
        {
            base.Start();
        }

        void OnGlide(Vector3 direction)
        {
            if (!enabled) return;
            var tracker = GestureTracker.Instance;
            if (
                tracker.leftTriggered ||
                tracker.rightTriggered ||
                tracker.leftGripped ||
                tracker.rightGripped) return;

            var player = Player.Instance;
            if (player.wasLeftHandTouching || player.wasRightHandTouching) return;

            var rigidbody = player.bodyCollider.attachedRigidbody;
            Vector3 velocity = direction * player.scale * speedScale;
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, velocity, acceleration);
        }

        List<LineRenderer> lines = new List<LineRenderer>();
        private void DrawRay(Vector3 position, Vector3 direction, Color color, int index)
        {
            try
            {
                LineRenderer renderer;
                if (index >= lines.Count)
                {
                    renderer = new GameObject($"Debug Line ({index})").AddComponent<LineRenderer>();
                    renderer.startWidth = .01f;
                    renderer.endWidth = .01f;
                    lines.Add(renderer);
                }
                else
                {
                    renderer = lines[index];
                }

                renderer.material.color = color;
                renderer.SetPosition(0, position);
                renderer.SetPosition(1, position + direction);
            }
            catch (Exception e)
            {
                Logging.LogException(e);
            }
        }

        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
            GestureTracker.Instance.OnGlide += OnGlide;
        }

        protected override void Cleanup()
        {
            GestureTracker.Instance.OnGlide -= OnGlide;
        }

        public override string DisplayName()
        {
            return "Airplane";
        }

        public override string Tutorial()
        {
            return "- To fly, do a T-pose (spread your arms out like wings on a plane). \n" +
                "- To fly up, rotate your palms so they face forward. \n" +
                "- To fly down, rotate your palms so they face backward.";
        }

    }*/

        Vector3 baseGravity;
        public float gravityScale = 0f;
        private bool flying = false;
        private bool gravity = true;

        public static int flySpeedIndex = 3;
        public static float[] speeds =
        {
            500f,
            1000f,
            1500f,
            2500f,
            3500f,
        };

        private bool leftSecondaryFrameDown = false;

        protected override void OnEnable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnEnable();
            baseGravity = UnityEngine.Physics.gravity;
        }
        protected override void OnDisable()
        {
            if (!MenuController.Instance.Built) return;
            base.OnDisable();
            UnityEngine.Physics.gravity = baseGravity;
        }
        void FixedUpdate()
        {
            if (this.enabled == true)
            {
                InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                bool rightHandSecondary = false;
                bool tempState = rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out rightHandSecondary);
                InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                bool leftHandSecondary = false;
                bool tempState1 = leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out leftHandSecondary);

                if (rightHandSecondary)
                {
                    Player.Instance.GetComponent<Rigidbody>().velocity =
                        Player.Instance.headCollider.transform.forward * Time.deltaTime * (speeds[flySpeedIndex]);
                    flying = true;
                }
                else if (flying == true)
                {
                    Player.Instance.GetComponent<Rigidbody>().velocity =
                        Player.Instance.headCollider.transform.forward * Time.deltaTime * 0f;
                    flying = false;
                }

                if (leftSecondaryFrameDown && leftHandSecondary)
                {
                    return;
                }
                else if (leftSecondaryFrameDown && !leftHandSecondary)
                {
                    leftSecondaryFrameDown = false;
                }

                if (leftHandSecondary && gravity)
                {
                    UnityEngine.Physics.gravity = baseGravity * gravityScale;
                    gravity = false;
                    leftSecondaryFrameDown = true;
                }
                else if (leftHandSecondary && !gravity)
                {
                    UnityEngine.Physics.gravity = baseGravity;
                    gravity = true;
                    leftSecondaryFrameDown = true;
                }
            }
        }
        public override string DisplayName()
        {
            return "SuperMan";
        }

        public override string Tutorial()
        {
            return "press button on right hand stink bozo fart";
        }

        protected override void Cleanup()
        {
            return;
        }
    }
}


