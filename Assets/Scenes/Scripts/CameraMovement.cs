using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Animator camAnimator;
    public bool camStartsOnLeft;
    Transform camPos;
    bool camIsRight = false;
    bool camIsLeft = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camAnimator = gameObject.GetComponent<Animator>();
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
}
