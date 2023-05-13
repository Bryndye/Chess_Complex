using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    NoEvent,
    RandomCard,
    Shop,
    Event,
    Key
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

    private void Awake()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
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
            case TileType.RandomCard:
                playerManager.AddCard(EventsManager.Instance.RandomCard());
                break;
            case TileType.Event:
                Debug.Log(playerManager.gameObject + " triggers EVENT");
                break;
            case TileType.Shop:
                Debug.Log(playerManager.gameObject + " shop!");
                break;
            case TileType.Key:
                Debug.Log(playerManager.gameObject + " gets the key!");
                break;
            default:
                break;
        }
    }

    public void SetValueToShader(string _name, int _value)
    {
        myMeshRenderer.material.SetInt(_name, _value);
    }

    private void OnMouseEnter()
    {
        //Debug.Log("Souris entrée sur l'objet : " + gameObject.name);
        SetValueToShader("_IsHover", 1);
    }

    private void OnMouseExit()
    {
        //Debug.Log("Souris sortie de l'objet : " + gameObject.name);
        SetValueToShader("_IsHover", 0);
    }
}


/* REGLE
 * Si tile est utilisé pour LA PREMIERE FOIS, son effet est déclanché et effet = neutre après utilisation
 * Si la tile couleur == joueur color, le joueur en question gagne une carte PION, CHEVALIER
 */
