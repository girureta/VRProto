using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRProto
{
    public class KeyboardInput : MonoBehaviour
    {
        public VRPlayerBehaviour player;
        public Vector3 rotationSpeed = new Vector3(90.0f, 90.0f, 90.0f);
        public Vector3 translationSpeed = new Vector3(1.0f,1.0f,1.0f);

        public enum Target
        {
            head,
            leftHand,
            rightHand
        }

        public enum Mode
        {
            Rotation,
            Position
        }

        public enum TranslationPlane
        {
            XY,
            XZ,
        }

        public Target target = Target.head;
        public Mode mode = Mode.Rotation;
        public TranslationPlane translationPlane = TranslationPlane.XY;
        protected Vector3 lastMousePosition;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                target = Target.head;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                target = Target.leftHand;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                target = Target.rightHand;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (mode == Mode.Position)
                {
                    mode = Mode.Rotation;
                }
                else
                {
                    mode = Mode.Position;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (translationPlane == TranslationPlane.XY)
                {
                    translationPlane = TranslationPlane.XZ;
                }
                else
                {
                    translationPlane = TranslationPlane.XY;
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                lastMousePosition = Input.mousePosition;

                switch (mode)
                {
                    case Mode.Rotation:
                        RotateTarget(GetTarget(), delta);
                        break;
                    case Mode.Position:
                        TranslateTarget(GetTarget(), delta);
                        break;
                    default:
                        break;
                }
                
            }

            CheckLeftController();
            CheckRightController();
        }

        protected void CheckLeftController()
        {
            var leftController = player.GetLeftControllerInput();
            CheckLeftControllerKeys(leftController, KeyCode.T, ButtonType.Trigger);
            CheckLeftControllerKeys(leftController, KeyCode.G, ButtonType.Grip);
        }

        protected void CheckRightController()
        {
            var rightController = player.GetRightControllerInput();
            CheckLeftControllerKeys(rightController, KeyCode.Y, ButtonType.Trigger);
            CheckLeftControllerKeys(rightController, KeyCode.H, ButtonType.Grip);
        }

        protected void CheckLeftControllerKeys(ControllerInput controllerInput,KeyCode keyCode, ButtonType buttonType)
        {
            if (Input.GetKeyUp(keyCode))
            {
                controllerInput.SetButtonState(buttonType, ButtonState.Pressed);
            }
        }

        protected Transform GetTarget()
        {
            Transform t = null;
            switch (target)
            {
                case Target.head:
                    t = player.head;
                    break;
                case Target.leftHand:
                    t = player.leftHand;
                    break;
                case Target.rightHand:
                    t = player.rightHand;
                    break;
                default:
                    t = player.head;
                    break;
            }
            return t;
        }

        protected void TranslateTarget(Transform t, Vector3 delta)
        {
            delta.Normalize();
            Vector3 displacement = translationSpeed;
            displacement.Scale(delta);
            displacement *= Time.deltaTime;
            displacement.z = 0.0f;

            Vector3 currentPosition = t.position;
            Vector3 newPosition = Vector3.zero;

            switch (translationPlane)
            {
                case TranslationPlane.XY:
                    newPosition = new Vector3(currentPosition.x + displacement.x, currentPosition.y + displacement.y, currentPosition.z);
                    break;
                case TranslationPlane.XZ:
                    newPosition = new Vector3(currentPosition.x + displacement.x, currentPosition.y, currentPosition.z + displacement.y);
                    break;
                default:
                    break;
            }

            t.position = newPosition;
        }

        protected void RotateTarget(Transform t,Vector3 delta)
        {
            delta = new Vector3(-delta.y, delta.x, 0.0f);
            delta.Normalize();
            Vector3 euler = rotationSpeed;
            euler.Scale(delta);
            euler *= Time.deltaTime;
            euler.z = 0.0f;

            Vector3 currentEuler = t.eulerAngles;
            t.rotation = Quaternion.Euler(currentEuler.x+euler.x, currentEuler.y + euler.y,0.0f);
        }
    }
}
