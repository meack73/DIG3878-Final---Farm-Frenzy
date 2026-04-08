using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    public StoreManager storeManager;
    
    public void OnClick1() 
    { 
        
        monsterSpawner.selectedMonster = 0;
    } 

    public void OnClick2() 
    {
        monsterSpawner.selectedMonster = 1; 
    }

    public void OnClick3() 
    { 
        monsterSpawner.selectedMonster = 2; 
    }

    public void OnClick4() 
    { 
        monsterSpawner.selectedMonster = 3; 
    }

}
