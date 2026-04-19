using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public int playerId;

    private void OnTriggerEnter(Collider other)
    {
        MonsterBehavior monster = other.GetComponent<MonsterBehavior>();
        if (monster != null && monster.playerId != playerId)
        {
            monster.TakeDamage(3);
        }
    }
}