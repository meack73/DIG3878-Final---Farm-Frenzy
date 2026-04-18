using UnityEngine;
using System.Collections; 

public class FlowerBehavior : PlantBehavior
{
    private Animator animator;    
    private Rigidbody rb;

    public bool coinSpawn = false; 
    private float coinSpawnAnimationCooldown = 2.5f; 
    private float coinSpawnAnimationTimer = 0f;


    protected override void Start()
    {
        base.Start(); //runs parent class plant behavior (audiosource)
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.freezeRotation = true;
    }
    
    void Update()
    {
        if(coinSpawn)
        {
            CoinAnimation();
        } else
        {
            SetAnimationState("Idle");
        }
    }

// ------------------------------- Set states -------------------------------------------- //
    void SetAnimationState(string state)
    {
        animator.SetBool("Coin", state == "Coin");
        animator.SetBool("Idle", state == "Idle");
    }

    public void CoinAnimation()
    {
        coinSpawnAnimationTimer += Time.deltaTime;

        if (coinSpawn && coinSpawnAnimationTimer >= coinSpawnAnimationCooldown)
        {        
            if (coinSpawn)
            {
                SetAnimationState("Coin");
                coinSpawn = false;
            }
            
            coinSpawnAnimationTimer = 0f;
        }
    }
  
    protected override void Die ()
    {
        StartCoroutine(DieAnimation());
    }

    IEnumerator DieAnimation()
    {
        float duration = 0.7f;
        float time = 0f;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(90, 0, 0);

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