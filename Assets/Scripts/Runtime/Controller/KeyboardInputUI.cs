using UnityEngine;
using UnityEngine.UI;

namespace VRProto
{
    public class KeyboardInputUI : MonoBehaviour
    {
        public KeyboardInput keyboardInput;

        public Text targetText;
        public Text modeText;
        public Text translationPlaneText;

        // Update is called once per frame
        void Update()
        {
            targetText.text = keyboardInput.target.ToString();
            modeText.text = keyboardInput.mode.ToString();
            translationPlaneText.text = keyboardInput.translationPlane.ToString();
        }
    }
}
