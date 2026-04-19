using UnityEngine;
using System.Collections; 

public class CherryBehavior : PlantBehavior
{
    private Animator animator;    
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start(); //runs parent class plant behavior (audiosource)
        health = 1;
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.freezeRotation = true;
    }
    
// ------------------------------- Set states -------------------------------------------- //
    void SetAnimationState(string state)
    {
        animator.SetBool("Idle", state == "Idle");
        animator.SetBool("Die", state == "Die");
    }

    protected override void Die()
    {
        SetAnimationState("Die");
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;
        StartCoroutine(PlaySoundAfterClip());
    }

    private IEnumerator PlaySoundAfterClip()
    {
        float clipLength = 1.6f;
        yield return new WaitForSeconds(clipLength);
        AudioSource.PlayClipAtPoint(death, transform.position);
        Destroy(gameObject, death.length);
    }
}