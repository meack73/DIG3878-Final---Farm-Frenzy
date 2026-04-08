using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class Launcher : MonoBehaviourPunCallbacks
{

    [Header("Room Settings")]
    [SerializeField] private byte maxPlayersPerRoom = 2;

    [Header("UI")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressLabel;
    [SerializeField] private GameObject waitingLabel;

    [Header("Scene Settings")]
    [SerializeField] private string gameplayScene = "SampleScene";

    //This is client's ver # so that users are separated from each other by gameVersion (allowing for breaking changes)
    private string gameVersion = "1";
    private bool isConnecting;  //bool to connect trigger scene change

    #region MonoBehaviorPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            OnJoinedRoom();
        }

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected: {cause}");
        isConnecting = false;
        progressLabel.SetActive(false);
        waitingLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No random room available, creating a new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    //temp variablee to test photon network
    int numJoined = 1;

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room. Player count = " + PhotonNetwork.CurrentRoom.PlayerCount);

        //Debug.Log("Joined room. Player count = " + ++numJoined);

        numJoined = PhotonNetwork.CurrentRoom.PlayerCount;

        progressLabel.SetActive(false);
        controlPanel.SetActive(false);
        isConnecting = true;

        if (numJoined < maxPlayersPerRoom)
        {
            waitingLabel.SetActive(true);

            Debug.Log("Waiting for another player...");
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered: " + newPlayer.NickName);

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {
            StartGame();
        }
    }

    #endregion

    #region Public Methods

    void Awake()
    {
        //makes sure we can use PhotonNewtwork.LoadLevel() on master & players can sync their level
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        waitingLabel.SetActive(false);
    }

    //connection process if connected, join random room. if not, connect to instance in Photon Cloud Network
    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        isConnecting = true;

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Connected to Master");
            OnConnectedToMaster();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Not connected to master, trying again...");
        }
    }

    void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Not master client...");
            return;
        }

        //waitingLabel.SetActive(false);

        if (numJoined < maxPlayersPerRoom)
        {
            Debug.Log("Still waiting on player to join...");
            return;
        }

        Debug.Log("Enough players joined. Loading game scene...");
        PhotonNetwork.LoadLevel(gameplayScene);
    }

    #endregion
}
