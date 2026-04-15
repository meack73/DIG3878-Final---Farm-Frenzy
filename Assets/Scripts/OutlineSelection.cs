using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    StoreManager storeManager;
    GameManager gameManager;
    private MonsterSpawner monsterSpawner;
    private string tileTag = "P1Tile"; 
    

    void Start()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
        
        gameManager = gm.GetComponent<GameManager>();
        storeManager = gameManager.GetComponent<StoreManager>();


        if (gameManager.playerId == 2)
        {
            tileTag = "P2Tile"; 
        }

        string boardTag = gameManager.playerId == 1 ? "P1Board" : "P2Board";
        GameObject board = GameObject.FindGameObjectWithTag(boardTag);
        monsterSpawner = board.GetComponentInChildren<MonsterSpawner>();

        if (monsterSpawner == null)
    Debug.LogError("MonsterSpawner not found for player " + gameManager.playerId);
else
    Debug.Log("Found spawner with playerId=" + monsterSpawner.playerId);
    }

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;

            TileData tile = highlight.GetComponentInParent<TileData>();

            if (highlight.CompareTag("Selectable") && tile != null && tile.CompareTag(tileTag) && highlight != selection)
            {
                Outline outline = highlight.GetComponent<Outline>();

                if (outline == null)
                {
                    outline = highlight.gameObject.AddComponent<Outline>();
                    outline.OutlineColor = Color.magenta;
                    outline.OutlineWidth = 7.0f;
                }

                outline.enabled = true;
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                if (selection)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                selection = highlight;
                selection.gameObject.GetComponent<Outline>().enabled = true;

               TileData tile = highlight.GetComponentInParent<TileData>();

                if (tile != null && tile.CompareTag(tileTag) && monsterSpawner != null)
                {

                    if (storeManager.plantPrice[storeManager.currentSelected].canBuy && monsterSpawner.selectedMonster != -1)
                    {
                        monsterSpawner.PlaceMonster(tile.xIndex, tile.zIndex);
                        monsterSpawner.speaker.PlayOneShot(monsterSpawner.placeSFX);

                        if (!storeManager.plantPrice[storeManager.currentSelected].onCooldown)
                        {
                            StartCoroutine(storeManager.BuyCooldown(storeManager.currentSelected));
                        }

                        Deselect();
                    }
                }
                
                //Debug.Log($"Selected tile at {tile.xIndex}, {tile.zIndex}");
                
                selection = raycastHit.transform;
                selection.gameObject.GetComponent<Outline>().enabled = true;
                highlight = null;
            }
            else
            {
                if (selection)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                }
            }
        }
    }

    void Deselect()
    {
        //yield return null;
        monsterSpawner.selectedMonster = -1;
    }

}
