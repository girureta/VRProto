using System.Collections;
using NUnit.Framework;
using NSubstitute;

namespace VRProto.Tests
{
    public class VRPlayerTests
    {
        [Test]
        public void ServerVRPlayerHasNullCamera()
        {
            var networkInterface = Substitute.For<INetworkInterface>();
            networkInterface.isClient.Returns(false);

            VRPlayer player = new VRPlayer();
            player.network = networkInterface;
            player.Start();

            Assert.IsNull(player.GetCamera());
        }

        [Test]
        public void ClientVRPlayerHasCamera()
        {
            var networkInterface = Substitute.For<INetworkInterface>();
            networkInterface.isClient.Returns(true);

            VRPlayer player = new VRPlayer();
            player.network = networkInterface;
            player.Start();

            Assert.IsNotNull(player.GetCamera());
        }
    }
}
