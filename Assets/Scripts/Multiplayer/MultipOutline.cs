using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MultipOutline : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;


    public MultipMonsterSpawner monsterSpawner;
    public MultipMonsterSpawner p1MonsterSpawner;
    public MultipMonsterSpawner p2MonsterSpawner;

    void Start()
    {
        int localPlayerID = Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber == 1 ? 1 : 2;
        if (localPlayerID == 1)
        {
            monsterSpawner = p1MonsterSpawner;
        }
        else
        {
            monsterSpawner = p2MonsterSpawner;
        }

        Debug.Log("Outline using spawner for Player " + localPlayerID);
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

                if (tile != null && monsterSpawner != null)
                {
                    monsterSpawner.PlaceMonster(tile.xIndex, tile.zIndex);
                    Debug.Log("TileData found: " + tile.name + ", " + tile.xIndex + ", " + tile.zIndex);

                }
                else
                {
                    Debug.Log("TileData or monsterSparner is missing");
                }

                Debug.Log($"Selected tile at {tile.xIndex}, {tile.zIndex}");

                //selection = raycastHit.transform;
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

}