using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    public MonsterSpawner monsterSpawner;
    StoreManager storeManager;

    void Start()
    {
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        storeManager = gameManager.GetComponent<StoreManager>();
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
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                    
                }
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

                Debug.Log("TileData found: " + tile.name + ", " + tile.xIndex + ", " + tile.zIndex);

                if (tile != null && monsterSpawner != null)
                {
                    if (storeManager.plantPrice[storeManager.currentSelected].canBuy)
                    {
                        monsterSpawner.PlaceMonster(tile.xIndex, tile.zIndex);
                        if (!storeManager.plantPrice[storeManager.currentSelected].onCooldown)
                        {
                            StartCoroutine(storeManager.BuyCooldown(storeManager.currentSelected));
                        }
                        Deselect();
                    }
                }
                
                Debug.Log($"Selected tile at {tile.xIndex}, {tile.zIndex}");
                
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
