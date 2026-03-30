using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    
    public void OnClick1() 
    { 
        monsterSpawner.selectedMonster = 0;
        Debug.Log("Selected Monster 0");
    } 

    public void OnClick2() 
    {
        monsterSpawner.selectedMonster = 1; 
        Debug.Log("Selected Monster 1");
    }

    public void OnClick3() 
    { 
        monsterSpawner.selectedMonster = 2; 
        Debug.Log("Selected Monster 2");
    }

}
