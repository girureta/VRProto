using UnityEngine;

namespace VRProto
{
    [System.Serializable]
    public class VRPlayer
    {
        public Transform head;
        public Transform leftHand;
        public Transform rightHand;

        public CameraSettings cameraSettings;
        protected Camera playerCamera;

        public INetworkInterface network;
        public ControllerInput leftControllerInput;
        public ControllerInput rightControllerInput;

        public void Start()
        {
            CheckRequiredData();
            PrepareCamera();
        }

 
        protected void CheckRequiredData()
        {
            CheckTransform(ref head, "Head");
            CheckTransform(ref leftHand, "LeftHand");
            CheckTransform(ref rightHand, "RightHand");
        }

        protected void CheckTransform(ref Transform trans,string name)
        {
            if (trans == null)
            {
                trans = new GameObject("name").transform;
            }
        }


        public Camera GetCamera()
        {
            return playerCamera;
        }

        protected void PrepareCamera()
        {
            if (network.isClient)
            {

                if (playerCamera == null)
                {
                    playerCamera = new GameObject("Camera").AddComponent<Camera>();
                    playerCamera.transform.SetParent(head);
                    playerCamera.transform.localRotation = Quaternion.identity;
                }

                if (cameraSettings == null)
                {
                    cameraSettings = ScriptableObject.CreateInstance<CameraSettings>();
                }

                playerCamera.nearClipPlane = cameraSettings.nearClipPlane;
                playerCamera.transform.localPosition = cameraSettings.offset;
            }
        }

        public void Update()
        {
            leftControllerInput.UpdateInput();
            rightControllerInput.UpdateInput();
        }

        public void LateUpdate()
        {
            leftControllerInput.LateUpdate();
            rightControllerInput.LateUpdate();
        }
    }
}
