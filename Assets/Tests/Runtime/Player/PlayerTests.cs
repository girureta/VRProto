using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Mirror;

namespace VRProto.Test
{
    public class PlayerTests
    {
        [UnityTest]
        public IEnumerator PlayerClientHasCamera()
        {
            InstantiatePlayer(TestResources.get.player.gameObject);
            yield return null;
            yield return null;
            VRPlayerBehaviour playerInstance = GameObject.FindObjectOfType<VRPlayerBehaviour>();
            Assert.IsNotNull(playerInstance.GetCamera());
        }

        protected static void InstantiatePlayer(GameObject player)
        {
            GameObject go = new GameObject("NetworkManager");
            go.SetActive(false);
            NetworkManager networkManager = go.AddComponent<NetworkManager>();
            networkManager.playerPrefab = player;
            networkManager.autoCreatePlayer = true;
            networkManager.OnValidate();
            go.SetActive(true);
            networkManager.StartHost();
        }
    }
}
