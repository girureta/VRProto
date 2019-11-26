using UnityEngine;

namespace VRProto
{
    [CreateAssetMenu(menuName = "VRProto/CameraSettings")]
    public class CameraSettings : ScriptableObject
    {
        public float nearClipPlane = 0.05f;
        public Vector3 offset = Vector3.zero;
    }
}
