using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class JoinSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string lobbyScene = "JoinMenu";
    [SerializeField] private GameObject personLeftWarning;

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
        Debug.Log($"{other.NickName} left the room");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}