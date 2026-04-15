using Photon.Pun;
using UnityEngine;
using ExitGames.Client.Photon;

public class FarmManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private CameraMovement cameraMovement;

    public bool isLeftSide;

    #region view points
    [SerializeField] private Transform LownView;
    [SerializeField] private Transform LcenterView;
    [SerializeField] private Transform LenemyView;

    [SerializeField] private Transform RownView;
    [SerializeField] private Transform RcenterView;
    [SerializeField] private Transform RenemyView;

    #endregion

    #region public Arrow Methods

    public void OnAKey()
    {
        if (cameraMovement == null)
        {
            return;
        }

        if (isLeftSide)
        {
            cameraMovement.MoveRight();
            Debug.Log("Left side: Move right");
        }
        else
        {
            cameraMovement.MoveLeft();
            Debug.Log("Right side: Move left");
        }
    }

    public void OnDKey()
    {
        if (cameraMovement == null)
        {
            return;
        }

        if (isLeftSide)
        {
            cameraMovement.MoveLeft();
            Debug.Log("Left side: Move left");
        }
        else
        {
            cameraMovement.MoveRight();
            Debug.Log("Right side: Move right");
        }
    }

    #endregion

    #region MonoBehaviour Callbacks
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cameraMovement == null)
        {
            Debug.Log("Missing camera");
            return;
        }
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);

        //if player entered first playerOne = true, if not playerOne = false
        //isLeftSide = PhotonNetwork.LocalPlayer.ActorNumber == 1;
        //isLeftSide = true; // for testing purposes
        //isLeftSide = false; // for testing purposes

        object sideValue;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("side", out sideValue))
        {
            string side = sideValue.ToString();
            isLeftSide = side == "left";
        }
        else
        {
            Debug.LogWarning("No side property found, falling back to ActorNumber.");
            isLeftSide = PhotonNetwork.LocalPlayer.ActorNumber == 1;
        }

        Debug.Log("Local nickname: " + PhotonNetwork.LocalPlayer.NickName);
        Debug.Log("ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber);
        Debug.Log("isLeftSide: " + isLeftSide);

        if (isLeftSide)
        {
            Debug.Log("Assigning left side views");
            cameraMovement.setViewPoints(LownView, LcenterView, LenemyView);
        }
        else
        {
            Debug.Log("Assigning right side views");
            cameraMovement.setViewPoints(RownView, RcenterView, RenemyView);
        }

        cameraMovement.SetStartSide();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnDKey();
            Debug.Log("D key pressed");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnAKey();
            Debug.Log("A key pressed");
        }
    }
    #endregion
};
