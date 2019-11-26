using Mirror;

namespace VRProto
{
    public class MirrorNetworkInterface : INetworkInterface
    {
        public NetworkBehaviour behaviour;
        public MirrorNetworkInterface(NetworkBehaviour behaviour)
        {
            this.behaviour = behaviour;
        }

        public bool isClient => behaviour.isClient;
    }
}
