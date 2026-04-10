using UnityEngine;

public class MiddleCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1Bullet") || other.CompareTag("P2Bullet"))
        {
            Destroy(other.gameObject);
        }
    }
}
