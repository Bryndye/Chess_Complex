using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EventRedType
{
    Switch,
    ArmadaRouge
}
public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;
    private TurnController turnController;
    private PlayerInterface playerInterface;

    public Card[] StockCards;

    [Header("Shop")]
    public bool IsShoping = false;
    [SerializeField] private CardShopInstance cardShoPrefab;
    [SerializeField] private GameObject shopInterface;
    [SerializeField] private Transform shopContainerCards;

    [Header("Event Red")]
    [SerializeField] private Sprite[] spritesRedEvent;
    [SerializeField] private ArmadaRouge prefabArmadaRouge;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        turnController = TurnController.instance;
        playerInterface = PlayerInterface.instance;
        shopInterface.SetActive(false);
    }

    public Card RandomCard()
    {
        int index = UnityEngine.Random.Range(0, StockCards.Length);
        return StockCards[index];
    }


    public void SetShop()
    {
        IsShoping = true;
        Card[] cards = new Card[] { RandomCard(), RandomCard(), RandomCard() };
        shopInterface.SetActive(true);
        foreach (var card in cards)
        {
            var cardShopInstance = Instantiate(cardShoPrefab, shopContainerCards);
            cardShopInstance.MyCard = card;
        }
    }
    public void SelectCardFromShop(Card _card)
    {
        shopInterface.SetActive(false);
        PlayerManager pm = turnController.Player1ManagerTurn();
        pm.AddCard(_card);
        playerInterface.AddCardContainer(_card, true);
        ClearContainer();
        IsShoping = false;
    }

    public void AddCardUI(Card _card)
    {
        playerInterface.AddCardContainer(_card, true);
    }

    private void ClearContainer()
    {
        int childCount = shopContainerCards.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            var cardInstance = shopContainerCards.GetChild(i).GetComponent<CardShopInstance>();
            Destroy(cardInstance.gameObject);
        }
    }

    private static readonly System.Random random = new System.Random();
    public void Event(Tile _tile)
    {
        Card _cardEventRed = new Card();      
        Array values = Enum.GetValues(typeof(EventRedType));
        EventRedType randomEnumValue = (EventRedType)values.GetValue(random.Next(values.Length));

        _cardEventRed.name = randomEnumValue.ToString();
        _cardEventRed.FrontSprite = new Sprite[1];
        _cardEventRed.FrontSprite[0] = spritesRedEvent[(int)randomEnumValue];

        switch (randomEnumValue)
        {
            case EventRedType.Switch:
                var tile1 = turnController.playerManager1.MyPlayer.currentTile;
                var tile2 = turnController.playerManager2.MyPlayer.currentTile;
                turnController.playerManager1.MyPlayer.SetItemOnTile(tile2);
                turnController.playerManager2.MyPlayer.SetItemOnTile(tile1);
                break;
            case EventRedType.ArmadaRouge:
                Debug.Log("ARMADA ROUGE");
                int _pawnsToSpawn = turnController.TurnCount <= 5 ? turnController.TurnCount : 5;
                for (int i = 0; i < _pawnsToSpawn; ++i)
                {
                    InstantiateArmadaRouge();
                }
                break;
            default:
                break;
        }

        TriggerEventUI(_tile.MyType, _cardEventRed);
    }

    private void InstantiateArmadaRouge()
    {
        var _armada = Instantiate(prefabArmadaRouge);
        TileController _tileC = TileController.instance;
        //foreach (Tile _tile in _tileC.tiles)
        //{
        //    if (!_tile.PlaceTaken)
        //    {
        //        _armada.SetItemOnTile(_tile);
        //        break;
        //    }
        //}
        System.Random random = new System.Random();
        Tile selectedTile;
        int attempts = 0;

        do
        {
            int randomIndex = random.Next(_tileC.tiles.Length);
            selectedTile = _tileC.tiles[randomIndex];
            attempts++;
        }
        while (selectedTile.PlaceTaken && attempts < 10); // Remplacez IsOccupied par la méthode/propriété correcte

        // Utilisez la position de la tuile sélectionnée pour créer le pion
        _armada.SetItemOnTile(selectedTile);
    }

    public void TriggerEventUI(TileType _tile, Card _card = null)
    {
        playerInterface.TriggerEventUI(_tile, _card);
    }
}
