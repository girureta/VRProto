using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace VRProto.Tests
{
    public class ControllerInputTests
    {
        //Checks that the button state wont change just after setting it.
        [Test]
        public void ButtonStateNoChange([Values(ButtonType.Grip, ButtonType.TouchPad, ButtonType.Trigger)]ButtonType buttonType)
        {
            ControllerInput controllerInput = new ControllerInput();
            var buttonState = ButtonState.Down;
            controllerInput.SetButtonState(buttonType, buttonState);

            Assert.AreEqual(controllerInput.GetButtonState(buttonType), ButtonState.Up);
        }

        [Test]
        public void ButtonStateChangeLateUpdate(
            [Values(ButtonType.Grip, ButtonType.TouchPad, ButtonType.Trigger)] ButtonType buttonType,
            [Values(ButtonState.Down, ButtonState.Pressed, ButtonState.Released)] ButtonState buttonState)
        {
            ControllerInput controllerInput = new ControllerInput();
            controllerInput.SetButtonState(buttonType, buttonState);
            controllerInput.LateUpdate();
            Assert.AreEqual(controllerInput.GetButtonState(buttonType), buttonState);
        }
    }
}
