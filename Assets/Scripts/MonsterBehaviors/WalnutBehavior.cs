using UnityEngine;
using System.Collections; 

public class WalnutBehavior : PlantBehavior
{
    private Animator animator;    
    private Rigidbody rb;

    public bool coinSpawn = false; 

    protected override void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.freezeRotation = true;
    }

    protected override void Die ()
    {
        StartCoroutine(DieAnimation());
    }

    IEnumerator DieAnimation()
    {
        float duration = 0.5f;
        float time = 0f;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(90, 0, 0);

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
        GetComponent<Collider>().enabled = false;
        audioSource.PlayOneShot(death);
        Destroy(gameObject, 2f);
    }
}