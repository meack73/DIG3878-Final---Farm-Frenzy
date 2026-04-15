using System.Collections;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    public StoreManager storeManager;
    public AudioClip canPlace;
    public AudioClip cantPlace;
    GameObject publicSpeaker;
    AudioSource speaker;
    
    void Awake()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("GameManager");
        
        publicSpeaker = GameObject.FindWithTag("Speaker");
        speaker = publicSpeaker.GetComponent<AudioSource>();
    }

    public void OnClick1() 
    { 
        //sets the id of the plant to shooter
        storeManager.CheckMonsterPrice(0);
        StartCoroutine(SelectPlant(0));
    } 

    public void OnClick2() 
    {
        //sets the id of the plant to cactus
        storeManager.CheckMonsterPrice(1);
        StartCoroutine(SelectPlant(1));
    }

    public void OnClick3() 
    { 
        //sets the id of the plant to mushroom
        storeManager.CheckMonsterPrice(2);
        StartCoroutine(SelectPlant(2));
    }

    public void OnClick4() 
    { 
        //sets the id of the plant to sunflower
        storeManager.CheckMonsterPrice(3);
        StartCoroutine(SelectPlant(3));
    }

    public void OnClick5() 
    { 
        //sets the id of the plant to walnut
        storeManager.CheckMonsterPrice(4);
        StartCoroutine(SelectPlant(4));
    }

    IEnumerator SelectPlant(int mID)
    {
        yield return null;
        if (storeManager.plantPrice[mID].canBuy)
        {
            monsterSpawner.selectedMonster = mID;
            StartCoroutine(storeManager.BuyCooldown(mID));
            speaker.PlayOneShot(canPlace);
        }
        else
        {
            Debug.Log("Player does not have enough money.");
            speaker.PlayOneShot(cantPlace);
        }
    }
    
}
