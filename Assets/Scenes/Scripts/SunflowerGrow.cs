using UnityEngine;
using System.Collections;

public class SunflowerGrow : MonoBehaviour
{
    //parts of sunflower for growth later
    public GameObject pot;
    public GameObject stem;
    public GameObject leaves;
    public GameObject sunHead;
    public GameObject petals;
    public GameObject soil;

    public float timeBetweenStages = 3f;
    public int healAmount = 10;
    public int coinAmount = 2;

    int currStage = 0;
    bool fullyGrown = false;
    bool isGrowing = false;

    //currency drop
    public GameObject sunCoinPrefab;
    public int numCoinsDrop = 2;
    public float dropRadius = 5f;
    public float coinSpawnHeight = 0.2f;
    public Collider flowerCol;

    public FlowerBehavior flowerBehavior;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flowerBehavior = GetComponent<FlowerBehavior>();
        flowerCol = GetComponent<Collider>();
        flowerCol.enabled = false;

        ResetSunflower();
        gameObject.SetActive(true); 
        if (gameObject.activeInHierarchy) {
            StartCoroutine(GrowSunflower());
        }
    }

    IEnumerator GrowSunflower()
    {
        isGrowing = true;
        fullyGrown = false;

        //Debug.Log("Sunflower: waiting to start growing...");

        // wait a few seconds before start growth
        //yield return new WaitForSeconds(7f);

        //set active pot & start growing
        pot.SetActive(true);

        //Debug.Log("Growing commences...");

        //wait a few seconds between stage 0 & stage 1
        //yield return new WaitForSeconds(7f);

        //Debug.Log("Sunflower has begun to grow - pot & soil appear");
        soil.SetActive(true);

        for (int i=0; i < 4; i++)
        {
            ShowOnlyStage(i);
            currStage = i;
            //Debug.Log("Sunflower is on growing stage: " + (i + 1));

            if (i < 3)
            {
                yield return new WaitForSeconds(timeBetweenStages);
            }

        }

        fullyGrown = true;
        isGrowing = false;
        flowerCol.enabled = true;

        //Debug.Log("Sunflower is fully grown & ready to be picked.");

        StartCoroutine(CoinDropLoop());
    }

    IEnumerator CoinDropLoop()
    {
        while (fullyGrown)
        {
            yield return new WaitForSeconds(3f);
            DropCoins();

        }

    }

    void ShowOnlyStage(int stageNum)
    {
        //1st growth stage only pot + stem
        if (stageNum == 0)
        {
            stem.SetActive(true);
        }
        //2nd growth stage only pot + stem + head
        else if (stageNum == 1)
        {
            sunHead.SetActive(true);
        }

        //3rd growth stage pot, petals, stem, head, leaves
        else if (stageNum == 2)
        {
            leaves.SetActive(true);
        }
        else if (stageNum == 3)
        {
            petals.SetActive(true);
        }
    }

    void ResetSunflower()
    {
        currStage = 0;
        fullyGrown = false;
        isGrowing = false;
        pot.SetActive(false);
        soil.SetActive(false);
        stem.SetActive(false);
        sunHead.SetActive(false);
        leaves.SetActive(false);
        petals.SetActive(false);
        flowerCol.enabled = false;

    }

    void OnMouseDown()
    {
        Debug.Log("Sunflower is clicked.");

        if (!fullyGrown)
        {
            Debug.Log("Not fully grown yet...");
            return;
        }

        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.Heal(healAmount);
            Debug.Log("Player healed!");
        }
        else
        {
            Debug.Log("No PlayerHealth found in scene :(");
        }

        StopAllCoroutines();
        ResetSunflower();
        StartCoroutine(GrowSunflower());
    }

    void DropCoins()
    {
        flowerBehavior.coinSpawn = true;
        if (sunCoinPrefab == null)
        {
            Debug.LogWarning("No sun coin assigned");
            return;
        }

        for (int i=0; i < numCoinsDrop; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-dropRadius, dropRadius), coinSpawnHeight, Random.Range(-dropRadius, dropRadius));

            Vector3 spawnPosition = transform.position + randomOffset;
            GameObject SunCoin = Instantiate(sunCoinPrefab, spawnPosition, Quaternion.identity);
            Destroy(SunCoin, 10f);
        }

        //Debug.Log("Coins dropped");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
