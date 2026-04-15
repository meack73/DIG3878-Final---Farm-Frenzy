using UnityEngine;
using Photon.Pun;

public class MultipDebugTest : MonoBehaviourPun
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Sending RPC...");
            photonView.RPC(nameof(TestRPC), RpcTarget.All);
        }
    }

    [PunRPC]
    void TestRPC()
    {
        Debug.Log("RPC RECEIVED on: " + PhotonNetwork.LocalPlayer.NickName);
    }
}
