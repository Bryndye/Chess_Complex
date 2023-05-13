using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    private TurnController turnController;

    [SerializeField] PlayerManager playerManager1;
    [SerializeField] PlayerManager playerManager2;

    private void Start()
    {
        turnController = TurnController.instance;
    }

    void Update()
    {
        mouseIntoWorld();
    }

    private void mouseIntoWorld()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // La souris est sur un objet UI, ignorer le raycast pour les objets 3D
            Debug.Log("UI entre souris et 3D");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject.TryGetComponent(out Tile _tile))
                {
                    //_tile.ChangerMyMaterial();
                    // FCT TEST PLAYER
                    if (turnController.Player1Turn())
                    {
                        playerManager1.MyPlayer.SetItemOnTile(_tile);
                    }
                    else
                    {
                        playerManager2.MyPlayer.SetItemOnTile(_tile);
                    } 
                }

                // Utilisez 'clickedObject' comme vous le souhaitez, par exemple :
                //Debug.Log("Objet cliqué : " + clickedObject.name);
            }
        }
    }
}
