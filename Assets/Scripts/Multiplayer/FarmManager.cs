using Photon.Pun;
using UnityEngine;

public class FarmManager : MonoBehaviourPunCallbacks
{

    public CameraMovement camera;

    private bool isPlayerOne;

    #region private Camera Update

    private void UpdateCameraView()
    {

        if (camera == null)
        {
            Debug.LogError("camera is NULL in FarmManager");
            return;
        }

        camera.SetStartSide(isPlayerOne);
    }

    #endregion

    #region public Arrow Methods

    public void OnAKey()
    {
        if (isPlayerOne)
        {
            camera.MoveRight();
        }
        else
        {
            camera.MoveLeft();
        }
    }

    public void OnDKey()
    {
        if (isPlayerOne)
        {
            camera.MoveLeft();
        }
        else
        {
            camera.MoveRight();
        }
    }

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (camera == null)
        {
            Debug.Log("Missing camera");
            return;
        }
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);

        //if player entered first playerOne = true, if not playerOne = false
        isPlayerOne = PhotonNetwork.LocalPlayer.ActorNumber == 1;

        camera.SetStartSide(isPlayerOne);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnDKey();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnAKey();
        }
    }
};
