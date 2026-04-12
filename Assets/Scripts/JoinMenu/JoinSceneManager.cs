using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class JoinSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string lobbyScene = "JoinMenu";
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(lobbyScene);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log($"{other.NickName} joined the room");
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat($"{other.NickName} left the room");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /*  //might not need for this project... yet...
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork: Trying to load a level but we are not the master client");
            return;
        }
        string sceneName = "Room For " + PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.LogFormat("PhotonNetwork: Loading Level: {sceneName}");
        PhotonNetwork.LoadLevel(sceneName);
    }
    */
}
