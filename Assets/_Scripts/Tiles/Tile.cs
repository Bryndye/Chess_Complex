using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    NoEvent,
    RandomCard,
    Shop,
    Event,
    Key,
    Out
}

public class Tile : MonoBehaviour
{
    [Header("Parameters")]
    public TileType MyType;
    public Vector2 MyPosition;
    public bool EffectUsed = false;
    public bool PlaceTaken = false;
    public int score = 0;

    [Space]
    [SerializeField] private Texture2D textureRandomCard;
    [SerializeField] private Texture2D textureShop;
    [SerializeField] private Texture2D textureEvent;
    [SerializeField] private Texture2D textureKey;

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
        var victory = VictoryManager.instance;
        if (MyType == TileType.Out && victory.eventKeyStarted)
        {
            playerManager.AddScoreToPlayer(10);
            victory.PlayerOnTileOut(playerManager, this);
            return;
        }
        if (EffectUsed)
        {
            return;
        }
        EffectUsed = true;
        myMeshRenderer.material.SetInt("_Show", 1);

        switch (MyType)
        {
            case TileType.NoEvent:
                break;
            case TileType.RandomCard:
                playerManager.AddCard(EventsManager.Instance.RandomCard());
                myMeshRenderer.material.SetTexture("_TextureShow", textureRandomCard);
                playerManager.AddScoreToPlayer(1);
                break;
            case TileType.Event:
                Debug.Log(playerManager.gameObject + " triggers EVENT");
                EventsManager.Instance.Event();
                myMeshRenderer.material.SetTexture("_TextureShow", textureEvent);
                playerManager.AddScoreToPlayer(2);
                break;
            case TileType.Shop:
                Debug.Log(playerManager.gameObject + " shop!");
                EventsManager.Instance.SetShop();
                myMeshRenderer.material.SetTexture("_TextureShow", textureShop);
                playerManager.AddScoreToPlayer(5);
                break;
            case TileType.Key:
                Debug.Log(playerManager.Player + " gets the key!");
                playerManager.GetKey();
                myMeshRenderer.material.SetTexture("_TextureShow", textureKey);
                playerManager.AddScoreToPlayer(20);
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