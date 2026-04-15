using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Launcher : MonoBehaviourPunCallbacks
{

    [Header("Room Settings")]
    [SerializeField] private byte maxPlayersPerRoom = 2;

    [Header("UI")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressLabel;
    [SerializeField] private GameObject waitingLabel;

    [Header("Scene Settings")]
    [SerializeField] private string gameplayScene = "SampleScene 1";

    //This is client's ver # so that users are separated from each other by gameVersion (allowing for breaking changes)
    private string gameVersion = "1";
    private bool isConnecting;  //bool to connect trigger scene change

    #region Public Methods

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
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
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom, IsVisible = true, IsOpen = true });

        Debug.Log("Joined room name: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("My actor number: " + PhotonNetwork.LocalPlayer.ActorNumber);
        Debug.Log("Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    //temp variablee to test photon network
    //int numJoined;

    public override void OnJoinedRoom()
    {
        
        Debug.Log("Joined room. Player count = " + PhotonNetwork.CurrentRoom.PlayerCount);

        //numJoined = PhotonNetwork.CurrentRoom.PlayerCount;

        progressLabel.SetActive(false);
        controlPanel.SetActive(false);

        /* 
         Debug.Log("Adding to Player Count for testing");
         numJoined++;
         */

        Hashtable props = new Hashtable();

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            props["side"] = "left";
        }
        else
        {
            props["side"] = "right";
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (PhotonNetwork.CurrentRoom.PlayerCount < maxPlayersPerRoom)
        {
            waitingLabel.SetActive(true);

            Debug.Log("Waiting for another player...");
        }
        else
        {
            waitingLabel.SetActive(false);
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered: " + newPlayer.NickName);

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom && PhotonNetwork.CurrentRoom != null)
        {
            StartGame();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Create room failed: {message} ({returnCode})");
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
        waitingLabel.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Join room failed: {message} ({returnCode})");
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
        waitingLabel.SetActive(false);
    }

    #endregion

    #region MonoBehaviorPunCallbacks Callbacks

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
        waitingLabel.SetActive(false);

        isConnecting = true;

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Connected to Master");
            PhotonNetwork.JoinRandomRoom();
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

        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("No room found.");
            return;
        }

        //waitingLabel.SetActive(false);

        if (PhotonNetwork.CurrentRoom.PlayerCount < maxPlayersPerRoom)
        {
            Debug.Log("Still waiting on player to join...");
            return;
        }

        Debug.Log("Enough players joined. Loading game scene...");
        PhotonNetwork.LoadLevel(gameplayScene);
        //PhotonNetwork.LoadLevel("SampleScene 1");
    }

    #endregion
}
