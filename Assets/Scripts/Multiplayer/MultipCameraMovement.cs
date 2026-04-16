using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class MultipCameraMovement : MonoBehaviour
{
    #region old camera movement code
    /*
    Animator camAnimator;
    //public bool camStartsOnLeft;
    Transform camPos;
    bool camIsRight = false;
    bool camIsLeft = false;

    public void SetStartSide(bool startOnLeft)
    {
        if (startOnLeft)
        {
            camAnimator.Play("Left");
            camIsLeft = true;
            camIsRight = false;
        }
        else
        {
            camAnimator.Play("Right");
            camIsLeft = false;
            camIsRight = true;
        }
    }

    public void MoveRight()
    {
        if (camIsLeft)
        {
            camAnimator.SetBool("Right", true);
            camAnimator.SetBool("Left", false);

            camIsLeft = false;
            camIsRight = true;
        }

    }

    public void MoveLeft()
    {
        if (camIsRight)
        {
            camAnimator.SetBool("Right", false);
            camAnimator.SetBool("Left", true);

            camIsLeft = true;
            camIsRight = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camAnimator = gameObject.GetComponent<Animator>();
        /*
        camPos = gameObject.transform;

        if (camStartsOnLeft)
        {
            camIsLeft = true;
        }
        else
        {
            camIsRight = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.D) && camIsLeft)
        {
            camAnimator.SetBool("Right", true);
            camAnimator.SetBool("Left", false);
            camIsLeft = false;
            camIsRight = true;
        }

        if (Input.GetKeyDown(KeyCode.A) && camIsRight)
        {
            camAnimator.SetBool("Left", true);
            camAnimator.SetBool("Right", false);
            camIsLeft = true;
            camIsRight = false;
        }

        
    }
    */
    #endregion

    #region view points
    [SerializeField] private Transform ownView;
    [SerializeField] private Transform centerView;
    [SerializeField] private Transform enemyView;

    #endregion

    #region movement & move vars
    [SerializeField] private float moveSpeed = 200f;

    private int currView = 0; // 0 = own, 1 = center, 2 = enemy
    private Coroutine moveCoroutine;

    #endregion

    #region public methods

    public void SetStartSide()
    {
        currView = 0;
        snapToCurrView();
    }

    public void MoveRight()
    {
        if (currView < 2)
        {
            currView++;
            moveToCurrView();
            Debug.Log("Moving right to view index " + currView);
        }
    }

    public void MoveLeft()
    {
        if (currView > 0)
        {
            currView--;
            moveToCurrView();
            Debug.Log("Moving left to view index " + currView);
        }
    }

    #endregion

    #region private methods

    private void snapToCurrView()
    {
        Transform target = getCurrTarget();

        if (target == null)
        {
            Debug.Log("CM: Missing target transform");
            return;
        }

        Debug.Log("Snapping to view index " + currView);

        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    private void moveToCurrView()
    {
        Transform target = getCurrTarget();

        if (target == null)
        {
            Debug.Log("CM: Missing target transform");
            return;
        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(SmoothMove(target));
        Debug.Log("Moving camera to: " + target.name +
          " position = " + target.position +
          " rotation = " + target.rotation.eulerAngles);
    }

    private Transform getCurrTarget()
    {
        switch (currView)
        {
            case 0:
                return ownView;
            case 1:
                return centerView;
            case 2:
                return enemyView;
            default:
                Debug.Log("CM: Invalid currView index");
                return null;
        }
    }

    private IEnumerator SmoothMove(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.01f || Quaternion.Angle(transform.rotation, target.rotation) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, moveSpeed * Time.deltaTime * 100f);

            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
        moveCoroutine = null;

        Debug.Log("Camera current position: " + transform.position);
    }

    #endregion

    public void setViewPoints(Transform own, Transform center, Transform enemy)
    {
        ownView = own;
        centerView = center;
        enemyView = enemy;

        Debug.Log("SetViewPoints called:");
        Debug.Log("Own = " + ownView.name);
        Debug.Log("Center = " + centerView.name);
        Debug.Log("Enemy = " + enemyView.name);
    }
}