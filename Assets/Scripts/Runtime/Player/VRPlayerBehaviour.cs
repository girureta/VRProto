using UnityEngine;
using Mirror;

namespace VRProto
{
    public class VRPlayerBehaviour : NetworkBehaviour
    {
        public Transform head;
        public Transform leftHand;
        public Transform rightHand;
        public GrabberBehaviour defaultLeftGrabber;
        public GrabberBehaviour defaultRightGrabber;
        public GrabberBehaviour currentLeftGrabber;
        public GrabberBehaviour currentRightGrabber;

        public CameraSettings cameraSettings;

        protected VRPlayer VRPlayer;

        public ButtonStateDictionaryItem leftButtonsStates;
        public ButtonStateDictionaryItem rightButtonsStates;

        private void Awake()
        {
            VRPlayer = new VRPlayer();
            VRPlayer.head = head;
            VRPlayer.leftHand = leftHand;
            VRPlayer.rightHand = rightHand;
            VRPlayer.cameraSettings = cameraSettings;
            VRPlayer.network = new MirrorNetworkInterface(this);
            VRPlayer.leftControllerInput = new ControllerInput();
            VRPlayer.rightControllerInput = new ControllerInput();

            currentLeftGrabber = defaultLeftGrabber;
            WearGrabber(currentLeftGrabber, leftHand,OnNewLeftGrabber);

            currentRightGrabber = defaultRightGrabber;
            WearGrabber(currentRightGrabber, rightHand, OnNewRightGrabber);
        }

        protected static void WearGrabber(GrabberBehaviour grabber,Transform pivot, System.Action<GrabberBehaviour> OnNewGrabber)
        {
            grabber.gameObject.SetActive(true);
            grabber.Wear(pivot);
            grabber.OnNewGrabber += OnNewGrabber;
        }

        public ControllerInput GetLeftControllerInput()
        {
            return VRPlayer.leftControllerInput;
        }

        public ControllerInput GetRightControllerInput()
        {
            return VRPlayer.rightControllerInput;
        }

        protected void OnNewLeftGrabber(GrabberBehaviour grabber)
        {
            currentLeftGrabber.OnNewGrabber -= OnNewLeftGrabber;
            if (currentLeftGrabber == defaultLeftGrabber)
            {
                currentLeftGrabber.gameObject.SetActive(false);
            }
            currentLeftGrabber = grabber;
            WearGrabber(currentLeftGrabber, leftHand, OnNewLeftGrabber);
        }

        protected void OnNewRightGrabber(GrabberBehaviour grabber)
        {
            currentRightGrabber.OnNewGrabber -= OnNewRightGrabber;
            if (currentRightGrabber == defaultRightGrabber)
            {
                currentRightGrabber.gameObject.SetActive(false);
            }
            currentRightGrabber = grabber;
            WearGrabber(currentRightGrabber, rightHand, OnNewRightGrabber);
        }

        private void Start()
        {
            VRPlayer.Start();
        }

        public Camera GetCamera()
        {
            return VRPlayer.GetCamera();
        }

        public void Update()
        {
            if (!isLocalPlayer)
                return;

            VRPlayer.Update();
            UpdateInputState(VRPlayer.leftControllerInput, leftButtonsStates);
            UpdateInputState(VRPlayer.rightControllerInput, rightButtonsStates);

            switch (currentLeftGrabber.GetState())
            {
                case GrabberBehaviour.GrabberState.free:
                    break;
                case GrabberBehaviour.GrabberState.hovering:
                    break;
                case GrabberBehaviour.GrabberState.worn:
                    break;
                case GrabberBehaviour.GrabberState.grabbingOtherObject:
                    break;
                default:
                    break;
            }

            if (leftButtonsStates[ButtonType.Trigger] == ButtonState.Pressed)
            {
                currentLeftGrabber.TryGrab();
            }

            if (leftButtonsStates[ButtonType.Grip] == ButtonState.Pressed)
            {
                switch (currentLeftGrabber.GetState())
                {
                    case GrabberBehaviour.GrabberState.worn:
                        if (currentLeftGrabber != defaultLeftGrabber)
                        {
                            currentLeftGrabber.UnWear();
                            currentLeftGrabber = defaultLeftGrabber;
                            WearGrabber(currentLeftGrabber, leftHand, OnNewLeftGrabber);
                        }
                        break;
                    case GrabberBehaviour.GrabberState.grabbingOtherObject:
                        currentLeftGrabber.Release();
                        break;
                    default:
                        break;
                }
            }

            if (rightButtonsStates[ButtonType.Trigger] == ButtonState.Pressed)
            {
                currentRightGrabber.TryGrab();
            }

            if (rightButtonsStates[ButtonType.Grip] == ButtonState.Pressed)
            {
                switch (currentRightGrabber.GetState())
                {
                    case GrabberBehaviour.GrabberState.worn:
                        if (currentRightGrabber != defaultRightGrabber)
                        {
                            currentRightGrabber.UnWear();
                            currentRightGrabber = defaultRightGrabber;
                            WearGrabber(currentRightGrabber, rightHand, OnNewRightGrabber);
                        }
                        break;
                    case GrabberBehaviour.GrabberState.grabbingOtherObject:
                        currentRightGrabber.Release();
                        break;
                    default:
                        break;
                }
            }
        }

        private void LateUpdate()
        {
            VRPlayer.LateUpdate();
        }

        protected static void UpdateInputState(ControllerInput controllerInput, ButtonStateDictionaryItem currentButtonsStates)
        {
            foreach (var key in controllerInput.currentButtonsStates.Keys)
            {
                if (!currentButtonsStates.ContainsKey(key))
                {
                    currentButtonsStates.Add(key, ButtonState.Up);
                }
                currentButtonsStates[key] = controllerInput.currentButtonsStates[key];
            }
        }

        public class ButtonStateDictionaryItem : SyncDictionary<ButtonType, ButtonState> { }
    }
}