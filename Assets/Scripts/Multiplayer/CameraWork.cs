using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

public class CameraWork : MonoBehaviour
{

    #region private View Variables

    private Transform leftViewPoint;
    private Transform centerViewPoint;
    private Transform rightViewPoint;
    private Transform targetView;

    #endregion

    #region private Speed Variables

    private float moveSpeed = 5f;
    private float rotateSpeed = 5f;

    #endregion

    #region public Set View Methods

    public void setToLeftSide()
    {
        targetView = leftViewPoint;
    }

    public void setToCenter()
    {
        targetView = centerViewPoint;
    }

    public void setToRightSide()
    {
        targetView = rightViewPoint;
    }

    public void snapToTarget()
    {
        if (targetView == null)
        {
            return;
        }

        transform.position = targetView.position;
        transform.rotation = targetView.rotation;
    }

    #endregion

    #region MonoBehavior CallBacks

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (targetView == null)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, targetView.position, moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetView.rotation, rotateSpeed * Time.deltaTime);

    }

    #endregion

}
