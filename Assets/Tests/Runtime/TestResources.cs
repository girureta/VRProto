using UnityEngine;
namespace VRProto.Test
{
    [CreateAssetMenu(menuName = "LudusPrueba/TestResources")]
    public class TestResources : ScriptableObject
    {

        protected static TestResources mTestResources = null;
        public static TestResources get
        { 
            get
            {
                if(mTestResources == null)
                {
                    mTestResources = Resources.Load<TestResources>("TestResources");
                }
                return mTestResources;
            }
        }
        public VRPlayerBehaviour player;
    } 
}
