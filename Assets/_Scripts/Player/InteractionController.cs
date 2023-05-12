using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    void Update()
    {
        mouseIntoWorld();
    }

    private void mouseIntoWorld()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;
            if (clickedObject.TryGetComponent(out Tile _tile))
            {
                _tile.ChangerMyMaterial();
                if (Input.GetMouseButtonDown(0)) // Vérifie le clic gauche de la souris
                {
                    // Utilisez 'clickedObject' comme vous le souhaitez, par exemple :
                    Debug.Log("Objet cliqué : " + clickedObject.name);
                }
            }
        }
    }
}
