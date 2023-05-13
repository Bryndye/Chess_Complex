using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    NoEvent,
    RandomEvent,
    Shop
}

public class Tile : MonoBehaviour
{
    [Header("Parameters")]
    public TileType MyType;
    public Vector2 MyPosition;
    public bool EffectUsed = false;
    public bool PlaceTaken = false;
    public int score = 0;

    private MeshRenderer myMeshRenderer;
    private Color originalColor;

    private void Awake()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        originalColor = myMeshRenderer.material.color;
    }

    public void EnterTile(PlayerManager playerManager = null)
    {
        PlaceTaken = true;
        if (playerManager != null)
        {
            UseEffect(playerManager);
        }
    }

    public void ExitTile()
    {
        PlaceTaken = false;
    }

    private void UseEffect(PlayerManager playerManager)
    {
        if (EffectUsed)
        {
            return;
        }
        EffectUsed = true;
        switch (MyType)
        {
            case TileType.NoEvent:
                break;
            case TileType.RandomEvent:
                playerManager.AddCard(EventsManager.Instance.RandomCard());
                break;
            case TileType.Shop:
                break;
            default:
                break;
        }
    }

    public void ChangerMyMaterial(Color color)
    {
        //Debug.Log("changed material ");
        myMeshRenderer.material.color = color;
    }

    private void OnMouseEnter()
    {
        // Code exécuté lorsque la souris entre dans le collider de l'objet
        //Debug.Log("Souris entrée sur l'objet : " + gameObject.name);
        ChangerMyMaterial(Color.red);
    }

    private void OnMouseExit()
    {
        // Code exécuté lorsque la souris quitte le collider de l'objet
        //Debug.Log("Souris sortie de l'objet : " + gameObject.name);
        ChangerMyMaterial(originalColor);
    }
}


/* REGLE
 * Si tile est utilisé pour LA PREMIERE FOIS, son effet est déclanché et effet = neutre après utilisation
 * Si la tile couleur == joueur color, le joueur en question gagne une carte PION, CHEVALIER
 */
