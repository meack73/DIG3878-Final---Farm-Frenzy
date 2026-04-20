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
        GetComponentInChildren<ExplosionTrigger>().playerId = playerId;
    }
    
// ------------------------------- Set states -------------------------------------------- //
    void SetAnimationState(string state)
    {
        animator.SetBool("Idle", state == "Idle");
        animator.SetBool("Die", state == "Die");
    }

    private IEnumerator PlaySoundAfterClip()
    {
        float clipLength = 1.6f;
        yield return new WaitForSeconds(clipLength);
        AudioSource.PlayClipAtPoint(death, transform.position);
    }

    private void Explode()
    {   
        StartCoroutine(ExpandSphere());
    }

    private IEnumerator ExpandSphere()
    {
        SphereCollider sphere = GetComponentInChildren<SphereCollider>();
        Transform sphereTransform = sphere.transform;
        Renderer rend = sphereTransform.GetComponent<Renderer>();
        Material mat = rend.material;

        float targetScale = 4.5f * 1.5f;
        float speed = 5f;

        while (sphereTransform.localScale.x < targetScale)
        {
            float growth = speed * Time.deltaTime;
            sphereTransform.localScale += new Vector3(growth, growth, growth);

            // fade out as it expands
            float alpha = Mathf.Lerp(1f, 0.5f, sphereTransform.localScale.x / targetScale);
            Color c = mat.color;
            mat.color = new Color(c.r, c.g, c.b, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }

    protected override void Die()
    {
        SetAnimationState("Die");
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;
        StartCoroutine(PlaySoundAfterClip());
        Explode();
    }

}