using Photon.Pun;
using UnityEngine;

public class FarmManager : MonoBehaviourPunCallbacks
{

    private CameraWork camera;

    private bool isPlayerOne;

    // 0 = own side, 1 = center, 2 = enemy side
    private int currView;

    #region private Camera Update

    private void UpdateCameraView()
    {
        if (isPlayerOne)
        {
            if (currView == 0)
            {
                camera.setToLeftSide();
            }
            else if (currView == 1)
            {
                camera.setToCenter();
            }
            else if (currView == 2)
            {
                camera.setToRightSide();
            }
        }
        else
        {
            if (currView == 0)
            {
                camera.setToRightSide();
            }
            else if (currView == 1)
            {
                camera.setToCenter();
            }
            else if (currView == 2)
            {
                camera.setToLeftSide();
            }
        }
    }

    #endregion

    #region public Pan Methods

    public void panTowardsCenterOrEnemy()
    {
        currView = Mathf.Min(currView + 1, 2);
        UpdateCameraView();
    }

    public void panTowardsFarm()
    {
        currView = Mathf.Max(currView - 1, 0);
        UpdateCameraView();
    }

    #endregion

    #region public Arrow Methods

    public void OnRightArrow()
    {
        if (isPlayerOne)
        {
            panTowardsCenterOrEnemy();
        }
        else
        {
            panTowardsFarm();
        }
    }

    public void OnLeftArrow()
    {
        if (isPlayerOne)
        {
            panTowardsFarm();
        }
        else
        {
            panTowardsCenterOrEnemy();
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

         //if player entered first playerOne = true, if not playerOne = false
         isPlayerOne = PhotonNetwork.LocalPlayer.ActorNumber == 1;

        currView = 0;
        UpdateCameraView();
        camera.snapToTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
