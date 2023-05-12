using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    public PlayerController playerTEST; // VAR TEST A SUPPRIMER

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
                    playerTEST.SetItemOnTile(_tile);
                }

                // Utilisez 'clickedObject' comme vous le souhaitez, par exemple :
                //Debug.Log("Objet cliqué : " + clickedObject.name);
            }
        }
    }
}
