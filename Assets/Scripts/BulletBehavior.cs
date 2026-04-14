using UnityEngine; 

public class DespawnTimer : MonoBehaviour {
    public float lifetime = 3.0f;

    void Start() {
        Destroy(gameObject, lifetime);
    }
}