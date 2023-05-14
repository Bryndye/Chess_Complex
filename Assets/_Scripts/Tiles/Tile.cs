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

    public void SetTexture()
    {
        switch (MyType)
        {
            case TileType.NoEvent:
                break;
            case TileType.RandomCard:
                myMeshRenderer.material.SetTexture("_TextureShow", textureRandomCard);
                Debug.Log(1);
                break;
            case TileType.Shop:
                myMeshRenderer.material.SetTexture("_TextureShow", textureShop);
                break;
            case TileType.Event:
                myMeshRenderer.material.SetTexture("_TextureShow", textureEvent);
                break;
            case TileType.Key:
                myMeshRenderer.material.SetTexture("_TextureShow", textureKey);
                break;
            case TileType.Out:
                break;
            default:
                break;
        }
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

        var _event = EventsManager.Instance;
        switch (MyType)
        {
            case TileType.NoEvent:
                break;
            case TileType.RandomCard:
                Card _card = _event.RandomCard();
                playerManager.AddCard(_card);
                playerManager.AddScoreToPlayer(1);
                _event.TriggerEventUI(MyType, _card);
                break;

            case TileType.Event:
                _event.Event();
                playerManager.AddScoreToPlayer(2);
                _event.TriggerEventUI(MyType);
                break;

            case TileType.Shop:
                _event.SetShop();
                playerManager.AddScoreToPlayer(5);
                _event.TriggerEventUI(MyType);
                break;

            case TileType.Key:
                playerManager.GetKey();
                playerManager.AddScoreToPlayer(20);
                _event.TriggerEventUI(MyType);
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
        //Debug.Log("Souris entr�e sur l'objet : " + gameObject.name);
        SetValueToShader("_IsHover", 1);
    }

    private void OnMouseExit()
    {
        //Debug.Log("Souris sortie de l'objet : " + gameObject.name);
        SetValueToShader("_IsHover", 0);
    }
}