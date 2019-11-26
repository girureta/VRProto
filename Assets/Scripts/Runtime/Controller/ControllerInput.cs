using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRProto
{
    [Serializable]
    public enum ButtonType
    {
        Trigger,
        Grip,
        TouchPad
    }

    [Serializable]
    public enum ButtonState
    {
        Up,
        Pressed,
        Down,
        Released
    }

    [System.Serializable]
    public class ControllerInput
    {
        protected static readonly ButtonType[] buttonTypes = { ButtonType.Trigger, ButtonType.Grip, ButtonType.TouchPad };
        public Dictionary<ButtonType, ButtonState> currentButtonsStates = new Dictionary<ButtonType, ButtonState>();
        public Dictionary<ButtonType, ButtonState> nextButtonsStates = new Dictionary<ButtonType, ButtonState>();

        public ControllerInput()
        {
            foreach (var buttonType in buttonTypes)
            {
                currentButtonsStates.Add(buttonType, ButtonState.Up);
                nextButtonsStates.Add(buttonType, ButtonState.Up);
            }
        }

        public ButtonState GetButtonState(ButtonType buttonType)
        {
            return currentButtonsStates[buttonType];
        }

        public void SetButtonState(ButtonType buttonType, ButtonState buttonState )
        {
            nextButtonsStates[buttonType]  = buttonState;
        }

        public void UpdateInput()
        {
            //Retrieve real input from a VR controller
        }

        public void LateUpdate()
        {
            foreach (var button in buttonTypes)
            {
                currentButtonsStates[button] = nextButtonsStates[button];
                nextButtonsStates[button] = ButtonState.Up;
            }
        }


    }
}
